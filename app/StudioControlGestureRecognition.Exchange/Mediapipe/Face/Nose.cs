using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace StudioControlGestureRecognition.Exchange.Mediapipe.Face
{
    public class Nose
    {
        public Vector3 Tip { get; set; } = Vector3.Zero;
        public Vector3 Top { get; set; } = Vector3.Zero;
        public Vector3 Bottom { get; set; } = Vector3.Zero;
        public Vector3[] PointingArch { get; set;} = Array.Empty<Vector3>();
        public Vector3[] Outer { get; set; } = Array.Empty<Vector3>();
    }
}
