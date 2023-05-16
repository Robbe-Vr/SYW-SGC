using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace StudioControlGestureRecognition.Exchange.Mediapipe.Pose
{
    public class Pose
    {
        public Arm[] Arms { get; set; } = Array.Empty<Arm>();
        public Leg[] Legs { get; set; } = Array.Empty<Leg>();
    }
}
