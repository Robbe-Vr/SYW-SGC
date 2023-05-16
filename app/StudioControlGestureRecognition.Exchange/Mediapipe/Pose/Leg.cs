using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace StudioControlGestureRecognition.Exchange.Mediapipe.Pose
{
    public class Leg
    {
        public Vector3 Hip { get; set; } = Vector3.Zero;
        public Vector3 Knee { get; set; } = Vector3.Zero;

        public Foot? Foot { get; set; } = new Foot();
    }
}
