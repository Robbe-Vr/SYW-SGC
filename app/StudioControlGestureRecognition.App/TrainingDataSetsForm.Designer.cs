namespace StudioControlGestureRecognition.App
{
    partial class TrainingDataSetsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            CreateLabelButton = new Button();
            CreateLabelInput = new TextBox();
            CreateLabelLabel = new Label();
            SelectLabelLabel = new Label();
            AvailableLabelsComboBox = new ComboBox();
            DataSetGroupLabel = new Label();
            DataSetGroupComboBox = new ComboBox();
            TrainingDataSetPictureBox = new PictureBox();
            RecordingCountDownTimer = new System.Windows.Forms.Timer(components);
            RecordingCountDownPanel = new Panel();
            panel1 = new Panel();
            RecordButton = new Button();
            AvailableDataSetsLabel = new Label();
            AvailableDataSetsComboBox = new ComboBox();
            DataSetInfoLabel = new Label();
            DeleteDataSetButton = new Button();
            DeleteLabelButton = new Button();
            TrainModelsButton = new Button();
            UpdateDataSetLabelButton = new Button();
            FrameCountLabel = new Label();
            TrainingDataSetVideoTimer = new System.Windows.Forms.Timer(components);
            PrevFrameButton = new Button();
            NextFrameButton = new Button();
            PlayPauseVideoButton = new Button();
            ((System.ComponentModel.ISupportInitialize)TrainingDataSetPictureBox).BeginInit();
            RecordingCountDownPanel.SuspendLayout();
            SuspendLayout();
            // 
            // CreateLabelButton
            // 
            CreateLabelButton.Enabled = false;
            CreateLabelButton.Location = new Point(12, 284);
            CreateLabelButton.Name = "CreateLabelButton";
            CreateLabelButton.Size = new Size(372, 35);
            CreateLabelButton.TabIndex = 22;
            CreateLabelButton.Text = "Create Label";
            CreateLabelButton.UseVisualStyleBackColor = true;
            CreateLabelButton.Click += CreateLabelButton_Click;
            // 
            // CreateLabelInput
            // 
            CreateLabelInput.Location = new Point(93, 255);
            CreateLabelInput.Name = "CreateLabelInput";
            CreateLabelInput.Size = new Size(291, 23);
            CreateLabelInput.TabIndex = 21;
            CreateLabelInput.TextChanged += CreateLabelInput_TextChanged;
            // 
            // CreateLabelLabel
            // 
            CreateLabelLabel.AutoSize = true;
            CreateLabelLabel.Location = new Point(12, 258);
            CreateLabelLabel.Name = "CreateLabelLabel";
            CreateLabelLabel.Size = new Size(75, 15);
            CreateLabelLabel.TabIndex = 20;
            CreateLabelLabel.Text = "Create Label:";
            // 
            // SelectLabelLabel
            // 
            SelectLabelLabel.AutoSize = true;
            SelectLabelLabel.Location = new Point(12, 169);
            SelectLabelLabel.Name = "SelectLabelLabel";
            SelectLabelLabel.Size = new Size(122, 15);
            SelectLabelLabel.TabIndex = 19;
            SelectLabelLabel.Text = "Label (to be assigned)";
            // 
            // AvailableLabelsComboBox
            // 
            AvailableLabelsComboBox.FormattingEnabled = true;
            AvailableLabelsComboBox.Location = new Point(12, 187);
            AvailableLabelsComboBox.Name = "AvailableLabelsComboBox";
            AvailableLabelsComboBox.Size = new Size(372, 23);
            AvailableLabelsComboBox.TabIndex = 18;
            AvailableLabelsComboBox.SelectedIndexChanged += AvailableLabelsComboBox_SelectedIndexChanged;
            // 
            // DataSetGroupLabel
            // 
            DataSetGroupLabel.AutoSize = true;
            DataSetGroupLabel.Location = new Point(12, 120);
            DataSetGroupLabel.Name = "DataSetGroupLabel";
            DataSetGroupLabel.Size = new Size(86, 15);
            DataSetGroupLabel.TabIndex = 24;
            DataSetGroupLabel.Text = "Data Set Group";
            // 
            // DataSetGroupComboBox
            // 
            DataSetGroupComboBox.FormattingEnabled = true;
            DataSetGroupComboBox.Location = new Point(12, 138);
            DataSetGroupComboBox.Name = "DataSetGroupComboBox";
            DataSetGroupComboBox.Size = new Size(372, 23);
            DataSetGroupComboBox.TabIndex = 23;
            DataSetGroupComboBox.SelectedIndexChanged += DataSetGroupComboBox_SelectedIndexChanged;
            // 
            // TrainingDataSetPictureBox
            // 
            TrainingDataSetPictureBox.BackColor = SystemColors.ActiveCaptionText;
            TrainingDataSetPictureBox.Location = new Point(390, 12);
            TrainingDataSetPictureBox.Name = "TrainingDataSetPictureBox";
            TrainingDataSetPictureBox.Size = new Size(398, 398);
            TrainingDataSetPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            TrainingDataSetPictureBox.TabIndex = 25;
            TrainingDataSetPictureBox.TabStop = false;
            // 
            // RecordingCountDownTimer
            // 
            RecordingCountDownTimer.Interval = 50;
            // 
            // RecordingCountDownPanel
            // 
            RecordingCountDownPanel.BackColor = SystemColors.ControlDark;
            RecordingCountDownPanel.Controls.Add(panel1);
            RecordingCountDownPanel.Location = new Point(12, 107);
            RecordingCountDownPanel.Name = "RecordingCountDownPanel";
            RecordingCountDownPanel.Size = new Size(372, 10);
            RecordingCountDownPanel.TabIndex = 27;
            // 
            // panel1
            // 
            panel1.Location = new Point(0, 14);
            panel1.Name = "panel1";
            panel1.Size = new Size(372, 10);
            panel1.TabIndex = 21;
            // 
            // RecordButton
            // 
            RecordButton.Enabled = false;
            RecordButton.Location = new Point(12, 66);
            RecordButton.Name = "RecordButton";
            RecordButton.Size = new Size(372, 48);
            RecordButton.TabIndex = 26;
            RecordButton.Text = "Record";
            RecordButton.UseVisualStyleBackColor = true;
            RecordButton.Click += RecordButton_Click;
            // 
            // AvailableDataSetsLabel
            // 
            AvailableDataSetsLabel.AutoSize = true;
            AvailableDataSetsLabel.Location = new Point(12, 329);
            AvailableDataSetsLabel.Name = "AvailableDataSetsLabel";
            AvailableDataSetsLabel.Size = new Size(55, 15);
            AvailableDataSetsLabel.TabIndex = 31;
            AvailableDataSetsLabel.Text = "Data Sets";
            // 
            // AvailableDataSetsComboBox
            // 
            AvailableDataSetsComboBox.FormattingEnabled = true;
            AvailableDataSetsComboBox.Location = new Point(12, 347);
            AvailableDataSetsComboBox.Name = "AvailableDataSetsComboBox";
            AvailableDataSetsComboBox.Size = new Size(372, 23);
            AvailableDataSetsComboBox.TabIndex = 30;
            AvailableDataSetsComboBox.SelectedIndexChanged += AvailableDataSetsComboBox_SelectedIndexChanged;
            // 
            // DataSetInfoLabel
            // 
            DataSetInfoLabel.AutoSize = true;
            DataSetInfoLabel.Location = new Point(12, 384);
            DataSetInfoLabel.Name = "DataSetInfoLabel";
            DataSetInfoLabel.Size = new Size(116, 15);
            DataSetInfoLabel.TabIndex = 32;
            DataSetInfoLabel.Text = "No DataSet Selected.";
            // 
            // DeleteDataSetButton
            // 
            DeleteDataSetButton.Location = new Point(12, 443);
            DeleteDataSetButton.Name = "DeleteDataSetButton";
            DeleteDataSetButton.Size = new Size(372, 35);
            DeleteDataSetButton.TabIndex = 33;
            DeleteDataSetButton.Text = "Delete DataSet";
            DeleteDataSetButton.UseVisualStyleBackColor = true;
            DeleteDataSetButton.Click += DeleteDataSetButton_Click;
            // 
            // DeleteLabelButton
            // 
            DeleteLabelButton.Enabled = false;
            DeleteLabelButton.Location = new Point(12, 214);
            DeleteLabelButton.Name = "DeleteLabelButton";
            DeleteLabelButton.Size = new Size(372, 35);
            DeleteLabelButton.TabIndex = 34;
            DeleteLabelButton.Text = "Delete Label";
            DeleteLabelButton.UseVisualStyleBackColor = true;
            DeleteLabelButton.Click += DeleteLabelButton_Click;
            // 
            // TrainModelsButton
            // 
            TrainModelsButton.Location = new Point(12, 12);
            TrainModelsButton.Name = "TrainModelsButton";
            TrainModelsButton.Size = new Size(372, 48);
            TrainModelsButton.TabIndex = 35;
            TrainModelsButton.Text = "Train Models";
            TrainModelsButton.UseVisualStyleBackColor = true;
            TrainModelsButton.Click += TrainModelsButton_Click;
            // 
            // UpdateDataSetLabelButton
            // 
            UpdateDataSetLabelButton.Location = new Point(12, 402);
            UpdateDataSetLabelButton.Name = "UpdateDataSetLabelButton";
            UpdateDataSetLabelButton.Size = new Size(372, 35);
            UpdateDataSetLabelButton.TabIndex = 36;
            UpdateDataSetLabelButton.Text = "Update Assigned Label";
            UpdateDataSetLabelButton.UseVisualStyleBackColor = true;
            UpdateDataSetLabelButton.Click += UpdateDataSetLabelButton_Click;
            // 
            // FrameCountLabel
            // 
            FrameCountLabel.AutoSize = true;
            FrameCountLabel.Location = new Point(390, 413);
            FrameCountLabel.Name = "FrameCountLabel";
            FrameCountLabel.Size = new Size(52, 15);
            FrameCountLabel.TabIndex = 37;
            FrameCountLabel.Text = "Frame: 0";
            // 
            // TrainingDataSetVideoTimer
            // 
            TrainingDataSetVideoTimer.Interval = 75;
            // 
            // PrevFrameButton
            // 
            PrevFrameButton.Location = new Point(390, 443);
            PrevFrameButton.Name = "PrevFrameButton";
            PrevFrameButton.Size = new Size(28, 35);
            PrevFrameButton.TabIndex = 38;
            PrevFrameButton.Text = "<";
            PrevFrameButton.UseVisualStyleBackColor = true;
            PrevFrameButton.Click += PrevFrameButton_Click;
            // 
            // NextFrameButton
            // 
            NextFrameButton.Location = new Point(414, 443);
            NextFrameButton.Name = "NextFrameButton";
            NextFrameButton.Size = new Size(28, 35);
            NextFrameButton.TabIndex = 39;
            NextFrameButton.Text = ">";
            NextFrameButton.UseVisualStyleBackColor = true;
            NextFrameButton.Click += NextFrameButton_Click;
            // 
            // PlayPauseVideoButton
            // 
            PlayPauseVideoButton.Location = new Point(448, 443);
            PlayPauseVideoButton.Name = "PlayPauseVideoButton";
            PlayPauseVideoButton.Size = new Size(48, 35);
            PlayPauseVideoButton.TabIndex = 40;
            PlayPauseVideoButton.Text = "Play";
            PlayPauseVideoButton.UseVisualStyleBackColor = true;
            PlayPauseVideoButton.Click += PlayPauseVideoButton_Click;
            // 
            // TrainingDataSetsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 486);
            Controls.Add(PlayPauseVideoButton);
            Controls.Add(NextFrameButton);
            Controls.Add(PrevFrameButton);
            Controls.Add(FrameCountLabel);
            Controls.Add(UpdateDataSetLabelButton);
            Controls.Add(TrainModelsButton);
            Controls.Add(DeleteLabelButton);
            Controls.Add(DeleteDataSetButton);
            Controls.Add(DataSetInfoLabel);
            Controls.Add(AvailableDataSetsLabel);
            Controls.Add(AvailableDataSetsComboBox);
            Controls.Add(RecordingCountDownPanel);
            Controls.Add(RecordButton);
            Controls.Add(TrainingDataSetPictureBox);
            Controls.Add(DataSetGroupLabel);
            Controls.Add(DataSetGroupComboBox);
            Controls.Add(CreateLabelButton);
            Controls.Add(CreateLabelInput);
            Controls.Add(CreateLabelLabel);
            Controls.Add(SelectLabelLabel);
            Controls.Add(AvailableLabelsComboBox);
            Name = "TrainingDataSetsForm";
            Text = "TrainingDataSetsForm";
            ((System.ComponentModel.ISupportInitialize)TrainingDataSetPictureBox).EndInit();
            RecordingCountDownPanel.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button CreateLabelButton;
        private TextBox CreateLabelInput;
        private Label CreateLabelLabel;
        private Label SelectLabelLabel;
        private ComboBox AvailableLabelsComboBox;
        private Label DataSetGroupLabel;
        private ComboBox DataSetGroupComboBox;
        private PictureBox TrainingDataSetPictureBox;
        private Panel RecordingCountDownPanel;
        private Panel panel1;
        private System.Windows.Forms.Timer RecordingCountDownTimer;
        private Button RecordButton;
        private Label AvailableDataSetsLabel;
        private ComboBox AvailableDataSetsComboBox;
        private Label DataSetInfoLabel;
        private Button DeleteDataSetButton;
        private Button DeleteLabelButton;
        private Button TrainModelsButton;
        private Button UpdateDataSetLabelButton;
        private Label FrameCountLabel;
        private System.Windows.Forms.Timer TrainingDataSetVideoTimer;
        private Button PrevFrameButton;
        private Button NextFrameButton;
        private Button PlayPauseVideoButton;
    }
}