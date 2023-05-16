using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace StudioControlGestureRecognition.Exchange.Mediapipe.Face
{
    public class Head
    {
        public float Width { get; set; } = float.MinValue;
        public float Height { get; set; } = float.MinValue;

        public Vector3 ChinBottom { get; set; } = Vector3.Zero;

        public Vector3[] Outer { get; set; } = Array.Empty<Vector3>();
        public Vector3 Center { get; set; } = Vector3.Zero;
    }
}
