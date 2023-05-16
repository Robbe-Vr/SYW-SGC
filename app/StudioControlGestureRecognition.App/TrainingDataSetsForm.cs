using OpenCvSharp.Extensions;
using StudioControlGestureRecognition.Core;
using StudioControlGestureRecognition.Core.Events;
using StudioControlGestureRecognition.Exchange.Gesture_Database;
using StudioControlGestureRecognition.Exchange.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudioControlGestureRecognition.App
{
    public partial class TrainingDataSetsForm : Form
    {
        private readonly AIDataManager _manager;

        private DateTime? _recordingStartedTime;

        private Bitmap[]? _dataSetVideo;
        private bool _dataSetVideoPaused = false;
        private int _dataSetVideoFrame = 0;

        public TrainingDataSetsForm(AIDataManager manager)
        {
            InitializeComponent();

            _manager = manager;

            AvailableLabelsComboBox.Items.AddRange(_manager.ListExistingLabels());
            if (AvailableLabelsComboBox.Items.Count > 0)
                AvailableLabelsComboBox.SelectedIndex = 0;

            AvailableDataSetsComboBox.Items.AddRange(_manager.ListTrainingSets());
            if (AvailableDataSetsComboBox.Items.Count > 0)
                AvailableDataSetsComboBox.SelectedIndex = AvailableDataSetsComboBox.Items.Count - 1;

            DataSetGroupComboBox.Items.AddRange(Enum.GetNames(typeof(DataSetGroup)).Select(g => g.Replace('_', ' ')).ToArray());
            DataSetGroupComboBox.SelectedIndex = (int)DataSetGroup.Gesture;

            _manager.RecordingActivated += RecordingActivated;

            _manager.RecordingCompleted += CleanUpRecordingCountDown;

            RecordingCountDownTimer.Tick += OnRecordingCountDownTick;

            TrainingDataSetVideoTimer.Tick += OnTrainingDataSetVideoTimerTick;
        }

        private void TrainModelsButton_Click(object sender, EventArgs e)
        {
            TrainModelsButton.Enabled = false;
            RecordButton.Enabled = false;

            _manager.TrainModels();
        }

        internal void TrainingCompleted()
        {
            TrainModelsButton.Enabled = true;
            RecordButton.Enabled = true;
        }

        public void EnableRecordButton()
        {
            RecordButton.Enabled = true;
        }

        public void DisableRecordButton()
        {
            RecordButton.Enabled = false;
        }

        private void AvailableLabelsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _manager.UpdateGestureLabel(AvailableLabelsComboBox.Text);

            DeleteLabelButton.Enabled = true;
        }

        private void CreateLabelInput_TextChanged(object sender, EventArgs e)
        {
            if (CreateLabelButton.Enabled && (String.IsNullOrWhiteSpace(CreateLabelInput.Text) || CreateLabelInput.Text.Length < 2))
            {
                CreateLabelButton.Enabled = false;
            }
            else if (!CreateLabelButton.Enabled && !String.IsNullOrWhiteSpace(CreateLabelInput.Text) && CreateLabelInput.Text.Length > 1)
            {
                CreateLabelButton.Enabled = true;
            }
        }

        private void CreateLabelButton_Click(object sender, EventArgs e)
        {
            if (_manager.CreateGestureLabel(CreateLabelInput.Text))
            {
                AvailableLabelsComboBox.Items.Clear();
                AvailableLabelsComboBox.Items.AddRange(_manager.ListExistingLabels());
                AvailableLabelsComboBox.SelectedIndex = 0;
            }

            CreateLabelInput.Text = string.Empty;
            CreateLabelButton.Enabled = false;
        }

        private void RecordButton_Click(object sender, EventArgs e)
        {
            if (RecordButton.Text == "Record")
            {
                DateTime? recordingStartedTime = _manager.StartRecording();
                if (recordingStartedTime != null)
                {
                    RecordButton.Text = "Cancel";
                    RecordButton.ForeColor = Color.OrangeRed;

                    Panel sliderPanel = new Panel();
                    RecordingCountDownPanel.Controls.Add(sliderPanel);
                    sliderPanel.Height = RecordingCountDownPanel.Height;
                    sliderPanel.Width = RecordingCountDownPanel.Width;
                    sliderPanel.Location = new Point(0, 0);

                    _recordingStartedTime = recordingStartedTime;
                    RecordingCountDownTimer.Start();
                }
            }
            else
            {
                if (RecordingCountDownTimer.Enabled)
                    RecordingCountDownTimer.Stop();

                _manager.CancelRecording();
            }
        }

        private void OnRecordingCountDownTick(object? sender, EventArgs e)
        {
            if (_recordingStartedTime == null) return;

            float progress = (DateTime.Now - _recordingStartedTime).Value.Milliseconds / 1_500;

            int width = (int)Math.Floor(RecordingCountDownPanel.Width * progress);

            RecordingCountDownPanel.Controls[0].BackColor = progress < 0.25 ? Color.Green : progress < 0.50 ? Color.Yellow : progress < 0.75 ? Color.Orange : Color.Red;
            RecordingCountDownPanel.Controls[0].Width = width;
            RecordingCountDownPanel.Controls[0].Location = new Point(RecordingCountDownPanel.Width - width, 0);
            RecordingCountDownPanel.Update();
        }

        private void RecordingActivated(object? sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                this?.Invoke(() =>
                {
                    if (RecordingCountDownTimer.Enabled)
                        RecordingCountDownTimer.Stop();
                    _recordingStartedTime = null;

                    RecordButton.Text = "Recording...";
                    RecordButton.ForeColor = Color.SkyBlue;

                    RecordingCountDownPanel.Controls.Clear();
                });
            }
            else
            {
                if (RecordingCountDownTimer.Enabled)
                    RecordingCountDownTimer.Stop();
                _recordingStartedTime = null;

                RecordButton.Text = "Recording...";
                RecordButton.ForeColor = Color.SkyBlue;

                RecordingCountDownPanel.Controls.Clear();
            }
        }

        private void CleanUpRecordingCountDown(object? sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                this?.Invoke(() =>
                {
                    if (RecordingCountDownTimer.Enabled)
                        RecordingCountDownTimer.Stop();
                    _recordingStartedTime = null;

                    RecordButton.Text = "Record";
                    RecordButton.ForeColor = Color.Black;

                    RecordingCountDownPanel.Controls.Clear();

                    AvailableDataSetsComboBox.Items.Clear();
                    AvailableDataSetsComboBox.Items.AddRange(_manager.ListTrainingSets());
                    if (AvailableDataSetsComboBox.Items.Count > 0)
                        AvailableDataSetsComboBox.SelectedIndex = AvailableDataSetsComboBox.Items.Count - 1;
                });
            }
            else
            {
                if (RecordingCountDownTimer.Enabled)
                    RecordingCountDownTimer.Stop();
                _recordingStartedTime = null;

                RecordButton.Text = "Record";
                RecordButton.ForeColor = Color.Black;

                RecordingCountDownPanel.Controls.Clear();

                AvailableDataSetsComboBox.Items.Clear();
                AvailableDataSetsComboBox.Items.AddRange(_manager.ListTrainingSets());
                if (AvailableDataSetsComboBox.Items.Count > 0)
                    AvailableDataSetsComboBox.SelectedIndex = AvailableDataSetsComboBox.Items.Count - 1;
            }
        }

        private void DataSetGroupComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TrainingDataSetVideoTimer.Enabled)
            {
                TrainingDataSetVideoTimer.Stop();
                _dataSetVideoFrame = 0;
                FrameCountLabel.Text = $"Frame: 0";
                _dataSetVideo?.All(v => { v.Dispose(); return true; });
                _dataSetVideo = null;

                PrevFrameButton.Enabled = false;
                NextFrameButton.Enabled = false;
                PlayPauseVideoButton.Enabled = false;
            }

            _manager.UpdateDataSetGroup((DataSetGroup)DataSetGroupComboBox.SelectedIndex);

            AvailableLabelsComboBox.SelectedText = string.Empty;
            AvailableLabelsComboBox.Items.Clear();
            AvailableLabelsComboBox.Items.AddRange(_manager.ListExistingLabels());
            if (AvailableLabelsComboBox.Items.Count > 0)
                AvailableLabelsComboBox.SelectedIndex = AvailableLabelsComboBox.Items.Count - 1;

            DeleteLabelButton.Enabled = false;
        }

        private void AvailableDataSetsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TrainingDataSetVideoTimer.Enabled)
            {
                TrainingDataSetVideoTimer.Stop();
                _dataSetVideoFrame = 0;
                FrameCountLabel.Text = $"Frame: 0";
                _dataSetVideo?.All(v => { v.Dispose(); return true; });
                _dataSetVideo = null;

                PrevFrameButton.Enabled = false;
                NextFrameButton.Enabled = false;
                PlayPauseVideoButton.Enabled = false;
            }

            if (DataSetGroupComboBox.SelectedIndex == (int)DataSetGroup.Gesture)
            {
                (Bitmap[] video, GestureClass gestureClass)? dataSet = _manager.TryLoadGestureTrainingSet(AvailableDataSetsComboBox.SelectedIndex);

                if (dataSet == null) return;

                PrevFrameButton.Enabled = true;
                NextFrameButton.Enabled = true;
                PlayPauseVideoButton.Enabled = true;

                TrainingDataSetVideoTimer.Start();
            }
            else
            {
                (Bitmap image, GestureClass gestureClass)? dataSet = _manager.TryLoadStaticGestureTrainingSet(AvailableDataSetsComboBox.SelectedIndex);

                if (dataSet == null) return;

                TrainingDataSetPictureBox.Image?.Dispose();
                TrainingDataSetPictureBox.Image = dataSet.Value.image;
                TrainingDataSetPictureBox.Update();

                DataSetInfoLabel.Text = $"Gesture Label: {dataSet.Value.gestureClass.Label} - Group: {dataSet.Value.gestureClass.Group}";
            }
        }

        private void OnTrainingDataSetVideoTimerTick(object? sender, EventArgs e)
        {
            if (_dataSetVideo == null)
            {
                TrainingDataSetVideoTimer.Stop();
                _dataSetVideoFrame = 0;
                return;
            }

            DisplayNextVideoFrame();
        }

        private void DisplayNextVideoFrame()
        {
            if (_dataSetVideo == null) { return; }

            _dataSetVideoFrame = _dataSetVideoFrame >= _dataSetVideo.Length - 2 ? 0 : _dataSetVideoFrame + 1;

            Bitmap frame = _dataSetVideo[_dataSetVideoFrame];

            TrainingDataSetPictureBox.Image?.Dispose();
            TrainingDataSetPictureBox.Image = frame;
            TrainingDataSetPictureBox.Update();

            FrameCountLabel.Text = $"Frame: {_dataSetVideoFrame + 1}";
        }

        private void PrevFrameButton_Click(object sender, EventArgs e)
        {
            if (TrainingDataSetVideoTimer.Enabled) return;

            _dataSetVideoFrame = _dataSetVideoFrame == 0 ? 21 : _dataSetVideoFrame - 2;

            DisplayNextVideoFrame();
        }

        private void NextFrameButton_Click(object sender, EventArgs e)
        {
            if (TrainingDataSetVideoTimer.Enabled) return;

            DisplayNextVideoFrame();
        }

        private void PlayPauseVideoButton_Click(object sender, EventArgs e)
        {
            if (_dataSetVideo == null) return;

            if (TrainingDataSetVideoTimer.Enabled)
            {
                TrainingDataSetVideoTimer.Stop();

                PlayPauseVideoButton.Text = "Play";
            }
            else
            {
                if (_dataSetVideoFrame < 0) _dataSetVideoFrame = 0;
                TrainingDataSetVideoTimer.Start();

                PlayPauseVideoButton.Text = "Pause";
            }
        }

        private void DeleteLabelButton_Click(object sender, EventArgs e)
        {
            if (_manager.DeleteCurrentLabel())
            {
                int index = AvailableLabelsComboBox.SelectedIndex;
                AvailableLabelsComboBox.SelectedText = string.Empty;
                AvailableLabelsComboBox.Items.Clear();
                AvailableLabelsComboBox.Items.AddRange(_manager.ListExistingLabels());
                if (AvailableLabelsComboBox.Items.Count - 1 > index)
                    AvailableLabelsComboBox.SelectedIndex = AvailableLabelsComboBox.Items.Count - 1;
                else if (AvailableLabelsComboBox.Items.Count - 1 < index)
                    AvailableLabelsComboBox.SelectedIndex = 0;
                else if (AvailableLabelsComboBox.Items.Count != 0)
                    AvailableLabelsComboBox.SelectedIndex = index;

                DeleteLabelButton.Enabled = false;
            }
        }

        private void UpdateDataSetLabelButton_Click(object sender, EventArgs e)
        {
            if (_manager.UpdateLabelForDataSet(AvailableDataSetsComboBox.SelectedIndex))
            {
                AvailableDataSetsComboBox.Items.Clear();
                AvailableDataSetsComboBox.Items.AddRange(_manager.ListTrainingSets());
            }
        }

        private void DeleteDataSetButton_Click(object sender, EventArgs e)
        {
            int index = AvailableDataSetsComboBox.SelectedIndex;
            if (_manager.DeleteDataSet(index))
            {
                AvailableDataSetsComboBox.Items.Clear();
                AvailableDataSetsComboBox.Items.AddRange(_manager.ListTrainingSets());
                if (AvailableDataSetsComboBox.Items.Count - 1 < index)
                    AvailableDataSetsComboBox.SelectedIndex = AvailableDataSetsComboBox.Items.Count - 1;
                else if (AvailableDataSetsComboBox.Items.Count - 1 < index)
                    AvailableDataSetsComboBox.SelectedIndex = 0;
                else if (AvailableDataSetsComboBox.Items.Count != 0)
                    AvailableDataSetsComboBox.SelectedIndex = index;
            }
        }
    }
}
