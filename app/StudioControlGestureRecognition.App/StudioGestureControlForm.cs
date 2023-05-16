using OpenCvSharp.Extensions;
using StudioControlGestureRecognition.Core;
using StudioControlGestureRecognition.Core.Events;
using StudioControlGestureRecognition.Exchange.Objects;

namespace StudioControlGestureRecognition.App
{
    public partial class StudioGestureControlForm : Form
    {
        private TrainingDataSetsForm _trainingDataSetsForm;

        private readonly AIDataManager _manager;

        private readonly VideoInputDevice[] _videoInputDevices;

        private bool _initializing = true;

        public StudioGestureControlForm()
        {
            InitializeComponent();

            _manager = new AIDataManager();

            _trainingDataSetsForm = new TrainingDataSetsForm(_manager);

            _videoInputDevices = _manager.GetVideoInputDevices().ToArray();

            VideoCameraSourceComboBox.Items.AddRange(_videoInputDevices.Select(cam => cam.Name).ToArray());

            VideoCameraSourceComboBox.SelectedIndex = String.IsNullOrEmpty(_manager.SelectedVideoInputDevice.Name) ? 0 : _videoInputDevices.ToList().FindIndex(x => x.Name == _manager.SelectedVideoInputDevice.Name);

            VideoDisplayModeComboBox.Items.AddRange(Enum.GetNames(typeof(VideoDisplayMode)));
            VideoDisplayModeComboBox.SelectedIndex = 0;

            VideoWidthInput.Value = _manager.VideoWidth;
            VideoHeightInput.Value = _manager.VideoHeight;

            VideoFPSInput.Value = _manager.VideoFPS;

            _manager.NextFrame += DisplayVideoFrame;

            _manager.TrainingCompleted += TrainingCompleted;

            _initializing = false;
        }

        private void TrainingCompleted(object? sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                this?.Invoke(new Action(() =>
                {
                    StartStopButton.Enabled = true;

                    _trainingDataSetsForm.TrainingCompleted();
                }));
            }
            else
            {
                StartStopButton.Enabled = true;

                _trainingDataSetsForm.TrainingCompleted();
            }
        }

        private void DisplayVideoFrame(object? sender, NextFrameEventArgs e)
        {
            if (_initializing) return;

            if (InvokeRequired)
            {
                this?.Invoke(new Action(() =>
                {
                    VideoInputPictureBox.Image?.Dispose();

                    VideoInputPictureBox.Image = e.Frame.ToBitmap();
                    e.Frame.Release();
                    e.Frame.Dispose();

                    DetectedGestureLabel.Text = $"Detected Gesture: {e.DetectedGestures.gesture ?? "Nothing"}";
                    PoseGestureLabel.Text = $"Pose: {e.DetectedGestures.pose ?? "Nothing"}";
                    FaceGestureLabel.Text = $"Face: {e.DetectedGestures.face ?? "Nothing"}";
                    LeftHandGestureLabel.Text = $"Left Hand: {e.DetectedGestures.leftHand ?? "Nothing"}";
                    RightHandGestureLabel.Text = $"Right Hand: {e.DetectedGestures.rightHand ?? "Nothing"}";

                    if (e.Error != null)
                    {
                        if (_manager.Stop())
                        {
                            StartStopButton.Text = "Start";
                            _trainingDataSetsForm.DisableRecordButton();

                            DetectedGestureLabel.Text = $"Detected Gesture: Nothing";
                            PoseGestureLabel.Text = $"Pose: Nothing";
                            FaceGestureLabel.Text = $"Face: Nothing";
                            LeftHandGestureLabel.Text = $"Left Hand: Nothing";
                            RightHandGestureLabel.Text = $"Right Hand: Nothing";
                        }
                    }
                }));
            }
            else
            {
                VideoInputPictureBox.Image?.Dispose();
                VideoInputPictureBox.Image = e.Frame.ToBitmap();
                e.Frame.Release();
                e.Frame.Dispose();

                DetectedGestureLabel.Text = $"Detected Gesture: {e.DetectedGestures.gesture ?? "Nothing"}";
                PoseGestureLabel.Text = $"Pose: {e.DetectedGestures.pose ?? "Nothing"}";
                FaceGestureLabel.Text = $"Face: {e.DetectedGestures.face ?? "Nothing"}";
                LeftHandGestureLabel.Text = $"Left Hand: {e.DetectedGestures.leftHand ?? "Nothing"}";
                RightHandGestureLabel.Text = $"Right Hand: {e.DetectedGestures.rightHand ?? "Nothing"}";

                if (e.Error != null)
                {
                    if (_manager.Stop())
                    {
                        StartStopButton.Text = "Start";
                        _trainingDataSetsForm.DisableRecordButton();

                        DetectedGestureLabel.Text = $"Detected Gesture: Nothing";
                        PoseGestureLabel.Text = $"Pose: Nothing";
                        FaceGestureLabel.Text = $"Face: Nothing";
                        LeftHandGestureLabel.Text = $"Left Hand: Nothing";
                        RightHandGestureLabel.Text = $"Right Hand: Nothing";
                    }
                }
            }
        }

        private void Form_Load(object sender, EventArgs e)
        {

        }

        private void From_OnFormClosing(object sender, FormClosingEventArgs e)
        {
            _manager.NextFrame -= DisplayVideoFrame;

            _trainingDataSetsForm?.Dispose();
            _manager.Dispose();
        }

        private void VideoCameraSourceComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_initializing) return;

            VideoInputDevice device = _videoInputDevices[VideoCameraSourceComboBox.SelectedIndex];

            if (device != null)
            {
                _manager.SetVideoCamera(device);
            }
        }

        private void VideoDisplayModeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_initializing) return;

            VideoDisplayMode mode = (VideoDisplayMode)VideoDisplayModeComboBox.SelectedIndex;

            _manager.SetVideoDisplayMode(mode);
        }

        private void VideoWidthInput_ValueChanged(object sender, EventArgs e)
        {
            int width = (int)VideoWidthInput.Value;
            _manager.VideoWidth = width;
        }

        private void VideoHeightInput_ValueChanged(object sender, EventArgs e)
        {
            int height = (int)VideoHeightInput.Value;
            _manager.VideoHeight = height;
        }

        private void VideoFPSInput_ValueChanged(object sender, EventArgs e)
        {
            _manager.UpdateVideoFPS((int)VideoFPSInput.Value);
        }

        private void StartStopButton_Click(object sender, EventArgs e)
        {
            if (StartStopButton.Text == "Start")
            {
                if (_manager.Start())
                {
                    StartStopButton.Text = "Stop";
                    _trainingDataSetsForm.EnableRecordButton();
                }
            }
            else
            {
                if (_manager.Stop())
                {
                    StartStopButton.Text = "Start";
                    _trainingDataSetsForm.DisableRecordButton();

                    DetectedGestureLabel.Text = $"Detected Gesture: Nothing";
                    PoseGestureLabel.Text = $"Pose: Nothing";
                    FaceGestureLabel.Text = $"Facee: Nothing";
                    LeftHandGestureLabel.Text = $"Left Hand: Nothing";
                    RightHandGestureLabel.Text = $"Right Hand: Nothing";
                }
            }
        }

        private void ToggleGestureDetectionButton_Click(object sender, EventArgs e)
        {
            if (!_manager.RecognitionEnabled)
            {
                _manager.EnableRecognition();

                ToggleGestureDetectionButton.Text = "Disable Gesture Detection";
            }
            else
            {
                _manager.DisableRecognition();

                ToggleGestureDetectionButton.Text = "Enable Gesture Detection";
            }
        }

        private void ManageTrainingDataSetsButton_Click(object sender, EventArgs e)
        {
            if (_trainingDataSetsForm == null || _trainingDataSetsForm.IsDisposed)
                _trainingDataSetsForm = new TrainingDataSetsForm(_manager);

            if (_trainingDataSetsForm.Visible)
                _trainingDataSetsForm.Hide();
            else _trainingDataSetsForm.Show();
        }
    }
}