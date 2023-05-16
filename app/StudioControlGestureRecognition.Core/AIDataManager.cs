using OpenCvSharp;
using StudioControlGestureRecognition.AI;
using StudioControlGestureRecognition.Core.Events;
using StudioControlGestureRecognition.Exchange.Gesture_Database;
using StudioControlGestureRecognition.Exchange.Mediapipe;
using StudioControlGestureRecognition.Exchange.Objects;
using StudioControlGestureRecognition.Exchange.Storage;
using StudioControlGestureRecognition.ImageProcessing;
using StudioControlGestureRecognition.Storage;
using System.Diagnostics;
using System.Drawing;
using System.Timers;

namespace StudioControlGestureRecognition.Core
{
    public class AIDataManager
    {
        private readonly StorageManager _storage;
        private readonly Preferences _preferences;

        private readonly VideoStreamCapturer _capture;

        public bool Running { get; private set; }
        public bool RecognitionEnabled { get; private set; }
        public bool Recording { get; private set; }
        private bool _recordFrameOpen = false;
        private bool _recordGestureOpen = false;
        private string _currentRecordingGestureLabel = string.Empty;
        private DataSetGroup _currentDataSetGroup = DataSetGroup.Gesture;

        private double[][] _gestureHistory = new double[21][] // 21 gesture sets: a gesture is captured every 70~80 ms -> 21 sets create a time set of ~1.575 seconds to capture a gesture
        {
            new double[4] { 0,0,0,0 }, new double[4] { 0,0,0,0 }, new double[4] { 0,0,0,0 }, new double[4] { 0,0,0,0 }, new double[4] { 0,0,0,0 }, new double[4] { 0,0,0,0 }, new double[4] { 0,0,0,0 },
            new double[4] { 0,0,0,0 }, new double[4] { 0,0,0,0 }, new double[4] { 0,0,0,0 }, new double[4] { 0,0,0,0 }, new double[4] { 0,0,0,0 }, new double[4] { 0,0,0,0 }, new double[4] { 0,0,0,0 },
            new double[4] { 0,0,0,0 }, new double[4] { 0,0,0,0 }, new double[4] { 0,0,0,0 }, new double[4] { 0,0,0,0 }, new double[4] { 0,0,0,0 }, new double[4] { 0,0,0,0 }, new double[4] { 0,0,0,0 },
        };
        private List<GestureDetectionTrackingData>? _capturedStaticGestureData;

        private readonly System.Timers.Timer _recordingTimer = new System.Timers.Timer(1_500);
        private readonly System.Timers.Timer _recordingActiveTimer = new System.Timers.Timer(1_600);

        public int VideoWidth { get { return _capture.Width; } set { _capture.Width = value; _imageProcessor.SetVideoShape(value, VideoHeight); } }
        public int VideoHeight { get { return _capture.Height; } set { _capture.Height = value; _imageProcessor.SetVideoShape(VideoWidth, value); } }
        public int VideoFPS { get { return _capture.FPS; } }

        private readonly ImageProcessor _imageProcessor;

        public event EventHandler<NextFrameEventArgs>? NextFrame;
        public event EventHandler? RecordingActivated;
        public event EventHandler? RecordingCompleted;
        public event EventHandler? TrainingCompleted;

        public VideoDisplayMode _videoDisplayMode = VideoDisplayMode.Normal;

        public VideoInputDevice SelectedVideoInputDevice { get { return _preferences.DefaultVideoCamera; } }

        private readonly AIModelManager _aiModelManager;

        public AIDataManager()
        {
            _storage = new StorageManager();

            _preferences = _storage.LoadPreferences();

            _imageProcessor = new ImageProcessor(_preferences.VideoWidth, _preferences.VideoHeight);

            _capture = new VideoStreamCapturer(_preferences.DefaultVideoCamera, _preferences.FPS, _preferences.VideoWidth, _preferences.VideoHeight);

            _aiModelManager = new AIModelManager(forceTrainModels: false, disableTrainingModels: true);

            _recordingTimer.AutoReset = false;
            _recordingTimer.Elapsed += OpenRecordingFrame;

            _recordingActiveTimer.AutoReset = false;
            _recordingActiveTimer.Elapsed += (sender, e) => { _recordGestureOpen = true; _capturedStaticGestureData = new List<GestureDetectionTrackingData>(); };
        }

        public void TrainModels()
        {
            bool restartAfterTraining = false;
            if (Running)
            {
                restartAfterTraining = true;
                Running = false;
            }

            if (Recording) DestroyRecording();

            _aiModelManager.TrainModels().ContinueWith((task) =>
            {
                string taskResult = task.IsFaulted ? $"Error: {task.Exception?.Message}\nSource: {task.Exception?.Source}\nStackTrace:\n{task.Exception?.StackTrace}" : $"Completed without errors.";

                Trace.WriteLine($"Training Finished for all models! Task: {taskResult}");

                TrainingCompleted?.Invoke(this, new EventArgs());

                if (restartAfterTraining)
                {
                    Start();
                }
            });
        }

        public bool EnableRecognition()
        {
            return !RecognitionEnabled && (RecognitionEnabled = true);
        }

        public bool DisableRecognition()
        {
            return RecognitionEnabled && !(RecognitionEnabled = false);
        }

        public DateTime? StartRecording()
        {
            if (!Running || Recording) return null;

            Recording = true;

            DateTime recordingStartTime = DateTime.Now;
            _recordingTimer.Start();
            
            return recordingStartTime;
        }

        private void OpenRecordingFrame(object? sender, ElapsedEventArgs e)
        {
            _recordFrameOpen = true;

            Recording = false;

            RecordingActivated?.Invoke(this, new EventArgs());
        }

        public bool CancelRecording()
        {
            DestroyRecording();

            RecordingCompleted?.Invoke(this, new EventArgs());

            return true;
        }

        private void DestroyRecording()
        {
            _recordingTimer.Stop();

            if (_recordFrameOpen) _recordFrameOpen = false;

            if (_capturedStaticGestureData != null)
            {
                _capturedStaticGestureData?.Clear();
                _capturedStaticGestureData = null;
            }

            Recording = false;
        }

        public string[] ListExistingLabels()
        {
            return _aiModelManager.ListLabels(_currentDataSetGroup);
        }

        public bool UpdateGestureLabel(string label)
        {
            _currentRecordingGestureLabel = label;

            return true;
        }

        public bool UpdateDataSetGroup(DataSetGroup group)
        {
            _currentDataSetGroup = group;

            return true;
        }

        public bool CreateGestureLabel(string label)
        {
            return _aiModelManager.CreateNewGestureClass(label, _currentDataSetGroup);
        }

        public IEnumerable<VideoInputDevice> GetVideoInputDevices()
        {
            return _capture.ListVideoInputDevices();
        }

        public bool SetVideoCamera(VideoInputDevice device)
        {
            if (_capture.SetDevice(device.Name))
            {
                _preferences.DefaultVideoCamera = device;
                _storage.StorePreferences(_preferences);

                return true;
            }
            else return false;
        }

        public bool UpdateVideoFPS(int fps)
        {
            if (_capture.UpdateFPS(fps))
            {
                _preferences.FPS = fps;
                _storage.StorePreferences(_preferences);

                return true;
            }
            else return false;
        }

        public void SetVideoDisplayMode(VideoDisplayMode mode)
        {
            _videoDisplayMode = mode;
        }

        public bool Start()
        {
            _imageProcessor.CheckOutsourcingServices();

            Running = true;

            int maxConsecutiveFails = 30;

            Task.Run(async () =>
            {
                int consecutiveFails = 0;
                string? error = null;

                while (Running)
                {
                    try
                    {
                        await ProcessNextFrame();

                        consecutiveFails = 0;
                    }
                    catch (Exception e)
                    {
                        consecutiveFails++;
                        if (consecutiveFails >= maxConsecutiveFails)
                        {
                            error = $"Failed to process a frame {maxConsecutiveFails} times in a row!\nShutting down...\nError: {e.Message}\nSource: {e.Source}\n";
                            Trace.WriteLine(error);

                            Running = false;
                        }
                        else Trace.WriteLine($"Failed to process frame!\nError: {e.Message}\nSource: {e.Source}\nConsecutive Fails: {consecutiveFails}");
                    }
                }

                NextFrame?.Invoke(this, new NextFrameEventArgs(_imageProcessor.GetBlackTemplate()) { Error = error });
            });

            return true;
        }

        public bool Stop()
        {
            Running = false;

            if (Recording) DestroyRecording();

            return true;
        }

        private async Task ProcessNextFrame()
        {
            Mat? frame = _capture.GetNextFrame();

            if (frame == null) return;

            (Mat processedImage, double[][] motion, IEnumerable<Human> humans) = await _imageProcessor.ProcessImage(frame, _videoDisplayMode);
            if (!frame.IsDisposed)
            {
                frame.Release();
                frame.Dispose();
            }
            frame = null;

            GestureDetectionTrackingData data = new GestureDetectionTrackingData(motion.Where(rect => rect[2] > _capture.Width / 30 && rect[3] > _capture.Height / 30).ToArray(),
                humans.SelectMany(h => h.PoseFlattened).ToArray(),
                humans.SelectMany(h => h.FaceFlattened).ToArray(),
                humans.SelectMany(h => h.LeftHandFlattened).ToArray(),
                humans.SelectMany(h => h.RightHandFlattened).ToArray());

            if (_recordFrameOpen)
            {
                _recordFrameOpen = false;
                await Task.Run(() => _aiModelManager.CreateTrainingData(data, _currentRecordingGestureLabel, _currentDataSetGroup));

                RecordingCompleted?.Invoke(this, new EventArgs());
            }

            (int poseGestureIndex, string? poseGesture, int faceGestureIndex, string? faceGesture, int leftHandGestureIndex, string? leftHandGesture, int rightHandGestureIndex, string? rightHandGesture, int gestureIndex, string? gesture, double[][] updatedHistory)? detections = null;
            if (RecognitionEnabled && !Recording)
            {
                detections = _aiModelManager.DetectGesture(data, _gestureHistory.ToArray());

                if (detections.HasValue)
                    _gestureHistory = detections.Value.updatedHistory;
            }

            if (Recording)
            {
                _capturedStaticGestureData?.Add(data);

                if (_recordGestureOpen && _capturedStaticGestureData != null)
                {
                    _recordFrameOpen = false;
                    await Task.Run(() => _aiModelManager.CreateGestureTrainingData(_capturedStaticGestureData.ToArray(), _gestureHistory, _currentRecordingGestureLabel, _currentDataSetGroup));

                    _capturedStaticGestureData.Clear();
                    _capturedStaticGestureData = null;

                    RecordingCompleted?.Invoke(this, new EventArgs());
                }
            }

            if (Running)
                NextFrame?.Invoke(this, new NextFrameEventArgs(processedImage, (detections?.poseGesture, detections?.faceGesture, detections?.leftHandGesture, detections?.rightHandGesture, detections?.gesture)));

            processedImage.Release();
            processedImage.Dispose();
        }

        public void Dispose()
        {
            _capture.Dispose();

            _imageProcessor.Dispose();

            _recordingTimer.Dispose();
        }

        public string[] ListTrainingSets()
        {
            return _aiModelManager.LoadTrainingSets().Select((s, index) => $"{index}. {s.GestureClass.Label}").ToArray();
        }

        public (Bitmap image, GestureClass gestureClass)? TryLoadStaticGestureTrainingSet(int index)
        {
            UnpreparedLabeledGestureSet? dataSet = _aiModelManager.LoadTrainingSets().ElementAtOrDefault(index);

            if (dataSet == null) return null;

            Bitmap img = _imageProcessor.DisplayDataOnImage(AIModelManager.SelectDataFromDataSet(dataSet.Data, dataSet.GestureClass.Group), dataSet.GestureClass.Group, VideoWidth, VideoHeight);

            return (img, dataSet.GestureClass);
        }

        public (Bitmap[] Video, GestureClass gestureClass)? TryLoadGestureTrainingSet(int index)
        {
            LabeledGestureSet? dataSet = _aiModelManager.LoadGetureTrainingSets().ElementAtOrDefault(index);

            if (dataSet == null) return null;

            List<Bitmap> video = new List<Bitmap>();

            foreach (GestureDetectionTrackingData dataSetPoint in dataSet.StaticGesturesData)
                video.Add(_imageProcessor.DisplayDataOnImage(dataSetPoint.HumanTracking, dataSet.GestureClass.Group, VideoWidth, VideoHeight));

            return (video.ToArray(), dataSet.GestureClass);
        }

        public bool DeleteCurrentLabel()
        {
            return _aiModelManager.DeleteLabel(_currentRecordingGestureLabel, _currentDataSetGroup);
        }

        public bool UpdateLabelForDataSet(int dataSetIndex)
        {
            if (dataSetIndex < 0) return false;

            return _aiModelManager.UpdateTrainingDataSet(dataSetIndex, _currentRecordingGestureLabel);
        }

        public bool DeleteDataSet(int dataSetIndex)
        {
            if (dataSetIndex < 0) return false;

            return _aiModelManager.DeleteTrainingDataSet(dataSetIndex);
        }
    }
}
