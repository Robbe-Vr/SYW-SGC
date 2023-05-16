using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace StudioControlGestureRecognition.Exchange.Mediapipe.Face
{
    public class Face
    {
        public Head Head { get; set; } = new Head();
        public Eye[] Eyes { get; set; } = Array.Empty<Eye>();
        public Nose Nose { get; set; } = new Nose();
        public Mouth Mouth { get; set; } = new Mouth();

        public Vector3[] LeftoverPoints { get; set; } = Array.Empty<Vector3>();
    }
}
