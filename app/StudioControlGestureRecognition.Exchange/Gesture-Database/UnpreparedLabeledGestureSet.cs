using StudioControlGestureRecognition.Exchange.Mediapipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudioControlGestureRecognition.Exchange.Gesture_Database
{
    public class UnpreparedLabeledGestureSet
    {
        public UnpreparedLabeledGestureSet() { }

        public GestureClass GestureClass { get; set; } = new GestureClass();

        public GestureDetectionTrackingData Data { get; set; } = new GestureDetectionTrackingData();
    }
}
