namespace StudioControlGestureRecognition.App
{
    partial class StudioGestureControlForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            VideoInputPictureBox = new PictureBox();
            VideoCameraSourceComboBox = new ComboBox();
            VideoCameraSourceLabel = new Label();
            VideoDisplayModeComboBox = new ComboBox();
            VideoDisplayModeLabel = new Label();
            VideoWidthInput = new NumericUpDown();
            VideoHeightInput = new NumericUpDown();
            VideoWidthInputLabel = new Label();
            VideoHeightInputLabel = new Label();
            VideoFPSInput = new NumericUpDown();
            VideoFPSInputLabel = new Label();
            StartStopButton = new Button();
            DetectedGestureLabel = new Label();
            ToggleGestureDetectionButton = new Button();
            ManageTrainingDataSetsButton = new Button();
            PoseGestureLabel = new Label();
            FaceGestureLabel = new Label();
            LeftHandGestureLabel = new Label();
            RightHandGestureLabel = new Label();
            ((System.ComponentModel.ISupportInitialize)VideoInputPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)VideoWidthInput).BeginInit();
            ((System.ComponentModel.ISupportInitialize)VideoHeightInput).BeginInit();
            ((System.ComponentModel.ISupportInitialize)VideoFPSInput).BeginInit();
            SuspendLayout();
            // 
            // VideoInputPictureBox
            // 
            VideoInputPictureBox.BackColor = SystemColors.ActiveCaptionText;
            VideoInputPictureBox.Location = new Point(12, 156);
            VideoInputPictureBox.Name = "VideoInputPictureBox";
            VideoInputPictureBox.Size = new Size(776, 776);
            VideoInputPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            VideoInputPictureBox.TabIndex = 0;
            VideoInputPictureBox.TabStop = false;
            // 
            // VideoCameraSourceComboBox
            // 
            VideoCameraSourceComboBox.FormattingEnabled = true;
            VideoCameraSourceComboBox.Location = new Point(417, 27);
            VideoCameraSourceComboBox.Name = "VideoCameraSourceComboBox";
            VideoCameraSourceComboBox.Size = new Size(371, 23);
            VideoCameraSourceComboBox.TabIndex = 1;
            VideoCameraSourceComboBox.SelectedIndexChanged += VideoCameraSourceComboBox_SelectedIndexChanged;
            // 
            // VideoCameraSourceLabel
            // 
            VideoCameraSourceLabel.AutoSize = true;
            VideoCameraSourceLabel.Location = new Point(417, 9);
            VideoCameraSourceLabel.Name = "VideoCameraSourceLabel";
            VideoCameraSourceLabel.Size = new Size(120, 15);
            VideoCameraSourceLabel.TabIndex = 2;
            VideoCameraSourceLabel.Text = "Video Camera Source";
            // 
            // VideoDisplayModeComboBox
            // 
            VideoDisplayModeComboBox.FormattingEnabled = true;
            VideoDisplayModeComboBox.Location = new Point(12, 78);
            VideoDisplayModeComboBox.Name = "VideoDisplayModeComboBox";
            VideoDisplayModeComboBox.Size = new Size(372, 23);
            VideoDisplayModeComboBox.TabIndex = 3;
            VideoDisplayModeComboBox.SelectedIndexChanged += VideoDisplayModeComboBox_SelectedIndexChanged;
            // 
            // VideoDisplayModeLabel
            // 
            VideoDisplayModeLabel.AutoSize = true;
            VideoDisplayModeLabel.Location = new Point(12, 60);
            VideoDisplayModeLabel.Name = "VideoDisplayModeLabel";
            VideoDisplayModeLabel.Size = new Size(112, 15);
            VideoDisplayModeLabel.TabIndex = 4;
            VideoDisplayModeLabel.Text = "Video Display Mode";
            // 
            // VideoWidthInput
            // 
            VideoWidthInput.Location = new Point(417, 78);
            VideoWidthInput.Maximum = new decimal(new int[] { 1920, 0, 0, 0 });
            VideoWidthInput.Minimum = new decimal(new int[] { 50, 0, 0, 0 });
            VideoWidthInput.Name = "VideoWidthInput";
            VideoWidthInput.Size = new Size(182, 23);
            VideoWidthInput.TabIndex = 5;
            VideoWidthInput.Value = new decimal(new int[] { 50, 0, 0, 0 });
            VideoWidthInput.ValueChanged += VideoWidthInput_ValueChanged;
            // 
            // VideoHeightInput
            // 
            VideoHeightInput.Location = new Point(607, 78);
            VideoHeightInput.Maximum = new decimal(new int[] { 1920, 0, 0, 0 });
            VideoHeightInput.Minimum = new decimal(new int[] { 50, 0, 0, 0 });
            VideoHeightInput.Name = "VideoHeightInput";
            VideoHeightInput.Size = new Size(181, 23);
            VideoHeightInput.TabIndex = 6;
            VideoHeightInput.Value = new decimal(new int[] { 50, 0, 0, 0 });
            VideoHeightInput.ValueChanged += VideoHeightInput_ValueChanged;
            // 
            // VideoWidthInputLabel
            // 
            VideoWidthInputLabel.AutoSize = true;
            VideoWidthInputLabel.Location = new Point(417, 60);
            VideoWidthInputLabel.Name = "VideoWidthInputLabel";
            VideoWidthInputLabel.Size = new Size(72, 15);
            VideoWidthInputLabel.TabIndex = 7;
            VideoWidthInputLabel.Text = "Video Width";
            // 
            // VideoHeightInputLabel
            // 
            VideoHeightInputLabel.AutoSize = true;
            VideoHeightInputLabel.Location = new Point(607, 60);
            VideoHeightInputLabel.Name = "VideoHeightInputLabel";
            VideoHeightInputLabel.Size = new Size(76, 15);
            VideoHeightInputLabel.TabIndex = 8;
            VideoHeightInputLabel.Text = "Video Height";
            // 
            // VideoFPSInput
            // 
            VideoFPSInput.Location = new Point(417, 127);
            VideoFPSInput.Maximum = new decimal(new int[] { 60, 0, 0, 0 });
            VideoFPSInput.Minimum = new decimal(new int[] { 5, 0, 0, 0 });
            VideoFPSInput.Name = "VideoFPSInput";
            VideoFPSInput.Size = new Size(182, 23);
            VideoFPSInput.TabIndex = 9;
            VideoFPSInput.Value = new decimal(new int[] { 5, 0, 0, 0 });
            VideoFPSInput.ValueChanged += VideoFPSInput_ValueChanged;
            // 
            // VideoFPSInputLabel
            // 
            VideoFPSInputLabel.AutoSize = true;
            VideoFPSInputLabel.Location = new Point(417, 109);
            VideoFPSInputLabel.Name = "VideoFPSInputLabel";
            VideoFPSInputLabel.Size = new Size(26, 15);
            VideoFPSInputLabel.TabIndex = 10;
            VideoFPSInputLabel.Text = "FPS";
            // 
            // StartStopButton
            // 
            StartStopButton.Location = new Point(12, 9);
            StartStopButton.Name = "StartStopButton";
            StartStopButton.Size = new Size(372, 48);
            StartStopButton.TabIndex = 11;
            StartStopButton.Text = "Start";
            StartStopButton.UseVisualStyleBackColor = true;
            StartStopButton.Click += StartStopButton_Click;
            // 
            // DetectedGestureLabel
            // 
            DetectedGestureLabel.AutoSize = true;
            DetectedGestureLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            DetectedGestureLabel.Location = new Point(197, 120);
            DetectedGestureLabel.Name = "DetectedGestureLabel";
            DetectedGestureLabel.Size = new Size(193, 21);
            DetectedGestureLabel.TabIndex = 18;
            DetectedGestureLabel.Text = "Detected Gesture: Nothing";
            // 
            // ToggleGestureDetectionButton
            // 
            ToggleGestureDetectionButton.Location = new Point(12, 115);
            ToggleGestureDetectionButton.Name = "ToggleGestureDetectionButton";
            ToggleGestureDetectionButton.Size = new Size(179, 35);
            ToggleGestureDetectionButton.TabIndex = 19;
            ToggleGestureDetectionButton.Text = "Enable Gesture Detection";
            ToggleGestureDetectionButton.UseVisualStyleBackColor = true;
            ToggleGestureDetectionButton.Click += ToggleGestureDetectionButton_Click;
            // 
            // ManageTrainingDataSetsButton
            // 
            ManageTrainingDataSetsButton.Location = new Point(607, 115);
            ManageTrainingDataSetsButton.Name = "ManageTrainingDataSetsButton";
            ManageTrainingDataSetsButton.Size = new Size(181, 35);
            ManageTrainingDataSetsButton.TabIndex = 20;
            ManageTrainingDataSetsButton.Text = "Manage Training DataSets";
            ManageTrainingDataSetsButton.UseVisualStyleBackColor = true;
            ManageTrainingDataSetsButton.Click += ManageTrainingDataSetsButton_Click;
            // 
            // PoseGestureLabel
            // 
            PoseGestureLabel.AutoSize = true;
            PoseGestureLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            PoseGestureLabel.Location = new Point(12, 935);
            PoseGestureLabel.Name = "PoseGestureLabel";
            PoseGestureLabel.Size = new Size(106, 21);
            PoseGestureLabel.TabIndex = 21;
            PoseGestureLabel.Text = "Pose: Nothing";
            // 
            // FaceGestureLabel
            // 
            FaceGestureLabel.AutoSize = true;
            FaceGestureLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            FaceGestureLabel.Location = new Point(228, 935);
            FaceGestureLabel.Name = "FaceGestureLabel";
            FaceGestureLabel.Size = new Size(104, 21);
            FaceGestureLabel.TabIndex = 22;
            FaceGestureLabel.Text = "Face: Nothing";
            // 
            // LeftHandGestureLabel
            // 
            LeftHandGestureLabel.AutoSize = true;
            LeftHandGestureLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            LeftHandGestureLabel.Location = new Point(417, 935);
            LeftHandGestureLabel.Name = "LeftHandGestureLabel";
            LeftHandGestureLabel.Size = new Size(141, 21);
            LeftHandGestureLabel.TabIndex = 23;
            LeftHandGestureLabel.Text = "Left Hand: Nothing";
            // 
            // RightHandGestureLabel
            // 
            RightHandGestureLabel.AutoSize = true;
            RightHandGestureLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            RightHandGestureLabel.Location = new Point(607, 939);
            RightHandGestureLabel.Name = "RightHandGestureLabel";
            RightHandGestureLabel.Size = new Size(152, 21);
            RightHandGestureLabel.TabIndex = 24;
            RightHandGestureLabel.Text = "Right Hand: Nothing";
            // 
            // StudioGestureControlForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(797, 969);
            Controls.Add(RightHandGestureLabel);
            Controls.Add(LeftHandGestureLabel);
            Controls.Add(FaceGestureLabel);
            Controls.Add(PoseGestureLabel);
            Controls.Add(ManageTrainingDataSetsButton);
            Controls.Add(ToggleGestureDetectionButton);
            Controls.Add(DetectedGestureLabel);
            Controls.Add(StartStopButton);
            Controls.Add(VideoFPSInputLabel);
            Controls.Add(VideoFPSInput);
            Controls.Add(VideoHeightInputLabel);
            Controls.Add(VideoWidthInputLabel);
            Controls.Add(VideoHeightInput);
            Controls.Add(VideoWidthInput);
            Controls.Add(VideoDisplayModeLabel);
            Controls.Add(VideoDisplayModeComboBox);
            Controls.Add(VideoCameraSourceLabel);
            Controls.Add(VideoCameraSourceComboBox);
            Controls.Add(VideoInputPictureBox);
            Name = "StudioGestureControlForm";
            Text = "StudioGestureControl";
            FormClosing += From_OnFormClosing;
            Load += Form_Load;
            ((System.ComponentModel.ISupportInitialize)VideoInputPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)VideoWidthInput).EndInit();
            ((System.ComponentModel.ISupportInitialize)VideoHeightInput).EndInit();
            ((System.ComponentModel.ISupportInitialize)VideoFPSInput).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox VideoInputPictureBox;
        private ComboBox VideoCameraSourceComboBox;
        private Label VideoCameraSourceLabel;
        private ComboBox VideoDisplayModeComboBox;
        private Label VideoDisplayModeLabel;
        private NumericUpDown VideoWidthInput;
        private NumericUpDown VideoHeightInput;
        private Label VideoWidthInputLabel;
        private Label VideoHeightInputLabel;
        private NumericUpDown VideoFPSInput;
        private Label VideoFPSInputLabel;
        private Button StartStopButton;
        private Label DetectedGestureLabel;
        private Button ToggleGestureDetectionButton;
        private Button ManageTrainingDataSetsButton;
        private Label PoseGestureLabel;
        private Label FaceGestureLabel;
        private Label LeftHandGestureLabel;
        private Label RightHandGestureLabel;
    }
}