using StudioControlGestureRecognition.Exchange.Mediapipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudioControlGestureRecognition.Exchange.Gesture_Database
{
    public class LabeledGestureSet : ILabeledDataSet
    {
        public LabeledGestureSet() { }

        public GestureClass GestureClass { get; set; } = new GestureClass();

        public double[][] Data { get; set; } = Array.Empty<double[]>();
        public GestureDetectionTrackingData[] StaticGesturesData { get; set; } = Array.Empty<GestureDetectionTrackingData>();
    }
}
