using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace StudioControlGestureRecognition.Exchange.Mediapipe.Hands
{
    public class Hand
    {
        public Vector3 Wrist { get; set; } = Vector3.Zero;

        public Finger Thumb { get; set; } = new Finger();
        public Finger Index { get; set; } = new Finger();
        public Finger Middle { get; set; } = new Finger();
        public Finger Ring { get; set; } = new Finger();
        public Finger Pinky { get; set; } = new Finger();
    }
}
