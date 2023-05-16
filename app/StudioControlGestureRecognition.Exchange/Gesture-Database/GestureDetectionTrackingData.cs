using StudioControlGestureRecognition.Exchange.Mediapipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudioControlGestureRecognition.Exchange.Gesture_Database
{
    public class GestureDetectionTrackingData
    {
        public GestureDetectionTrackingData() { }

        public GestureDetectionTrackingData(double[][] motion, double[][] humanPose, double[][] humanFace, double[][] humanLeftHand, double[][] humanRightHand)
        {
            MotionObjectsTracking = motion;
            HumanPoseTracking = humanPose;
            HumanFaceTracking = humanFace;
            HumanLeftHandTracking = humanLeftHand;
            HumanRightHandTracking = humanRightHand;
        }

        public double[][] MotionObjectsTracking { get; set; } = Array.Empty<double[]>();
        public double[][] HumanTracking { get { return HumanPoseTracking.Concat(HumanFaceTracking).Concat(HumanLeftHandTracking).Concat(HumanRightHandTracking).ToArray(); } }
        public double[][] HumanPoseTracking { get; set; } = Array.Empty<double[]>();
        public double[][] HumanFaceTracking { get; set; } = Array.Empty<double[]>();
        public double[][] HumanLeftHandTracking { get; set; } = Array.Empty<double[]>();
        public double[][] HumanRightHandTracking { get; set; } = Array.Empty<double[]>();
    }
}
