using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace StudioControlGestureRecognition.Exchange.Mediapipe.Pose
{
    public class Foot
    {
        public Vector3 Ankle { get; set; } = Vector3.Zero;
        public Vector3 Heel { get; set; } = Vector3.Zero;
        public Vector3 Toes { get; set; } = Vector3.Zero;
    }
}
