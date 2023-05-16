using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudioControlGestureRecognition.Exchange.Mediapipe
{
    public class Human
    {
        public Pose.Pose Pose { get; set; } = new Pose.Pose();
        public Face.Face Face { get; set; } = new Face.Face();
        public Hands.Hand[] Hands { get; set; } = Array.Empty<Hands.Hand>();

        public double[][] Flattened { get { return PoseFlattened.Concat(FaceFlattened).Concat(LeftHandFlattened).Concat(RightHandFlattened).ToArray(); } }
        public double[][] RightHandFlattened { get; set; } = Array.Empty<double[]>();
        public double[][] LeftHandFlattened { get; set; } = Array.Empty<double[]>();
        public double[][] FaceFlattened { get; set; } = Array.Empty<double[]>();
        public double[][] PoseFlattened { get; set; } = Array.Empty<double[]>();
    }
}
