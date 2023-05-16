using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudioControlGestureRecognition.Core.Events
{
    public class NextFrameEventArgs : EventArgs
    {
        public NextFrameEventArgs(Mat frame, (string? pose, string? face, string? leftHand, string? rightHand, string? gesture)? detections = null)
        {
            Frame = frame;
            DetectedGestures = detections ?? (null, null, null, null, null);
        }

        public Mat Frame { get; private set; }

        public (string? pose, string? face, string? leftHand, string? rightHand, string? gesture) DetectedGestures { get; private set; }

        public string? Error { get; set; }
    }
}
