using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace StudioControlGestureRecognition.Exchange.Mediapipe.Hands
{
    public class Finger
    {
        public Vector3 Mcp { get; set; } = Vector3.Zero;
        public Vector3 Pip { get; set; } = Vector3.Zero;
        public Vector3 Dip { get; set; } = Vector3.Zero;
        public Vector3 Tip { get; set; } = Vector3.Zero;
    }
}
