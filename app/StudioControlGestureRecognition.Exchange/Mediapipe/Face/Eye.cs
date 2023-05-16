using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace StudioControlGestureRecognition.Exchange.Mediapipe.Face
{
    public class Eye
    {
        public Vector3[] Outer { get; set; } = Array.Empty<Vector3>();
        public Vector3 Left { get; set; } = Vector3.Zero;
        public Vector3 Right { get; set; } = Vector3.Zero;
        public Vector3 Pupil { get; set; } = Vector3.Zero;
        public Vector3[] Brow { get; set; } = Array.Empty<Vector3>();
    }
}
