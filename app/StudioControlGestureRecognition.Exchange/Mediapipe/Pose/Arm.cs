using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace StudioControlGestureRecognition.Exchange.Mediapipe.Pose
{
    public class Arm
    {
        public Vector3 Shoulder { get; set; } = Vector3.Zero;
        public Vector3 Elbow { get; set; } = Vector3.Zero;
        public Vector3 Hand { get; set; } = Vector3.Zero;
    }
}
