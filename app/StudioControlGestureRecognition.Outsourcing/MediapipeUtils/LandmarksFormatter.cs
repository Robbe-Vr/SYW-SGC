using StudioControlGestureRecognition.Exchange.Mediapipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudioControlGestureRecognition.Exchange.Mediapipe.Pose;
using System.Numerics;
using StudioControlGestureRecognition.Exchange.Mediapipe.Hands;
using StudioControlGestureRecognition.Exchange.Mediapipe.Face;
using System.Reflection;
using System.Runtime.Intrinsics;

namespace StudioControlGestureRecognition.Outsourcing.MediapipeUtils
{
    internal static class LandmarksFormatter
    {
        static int _width;
        static int _height;

        internal static IEnumerable<Human> FormatResults(dynamic results, int width, int height)
        {
            _width = width;
            _height = height;

            List<Human> humans = new List<Human>();

            Human human = new Human();
            human.Pose = CreatePose(results.pose);
            human.Hands = new Hand[2] { CreateHand(results.left_hand), CreateHand(results.right_hand) };
            human.Face = CreateFace(results.face);
            human.PoseFlattened = new double[][]
            {
                // Pose
                AddToFlattenedList(human.Pose.Arms.ElementAtOrDefault(0)?.Shoulder),
                AddToFlattenedList(human.Pose.Arms.ElementAtOrDefault(0)?.Elbow),
                AddToFlattenedList(human.Pose.Arms.ElementAtOrDefault(0)?.Hand),

                AddToFlattenedList(human.Pose.Arms.ElementAtOrDefault(1)?.Shoulder),
                AddToFlattenedList(human.Pose.Arms.ElementAtOrDefault(1)?.Elbow),
                AddToFlattenedList(human.Pose.Arms.ElementAtOrDefault(1)?.Hand),

                AddToFlattenedList(human.Pose.Legs.ElementAtOrDefault(0)?.Hip),
                AddToFlattenedList(human.Pose.Legs.ElementAtOrDefault(0)?.Knee),
                AddToFlattenedList(human.Pose.Legs.ElementAtOrDefault(0)?.Foot?.Ankle),
                AddToFlattenedList(human.Pose.Legs.ElementAtOrDefault(0)?.Foot?.Heel),
                AddToFlattenedList(human.Pose.Legs.ElementAtOrDefault(0)?.Foot?.Toes),

                AddToFlattenedList(human.Pose.Legs.ElementAtOrDefault(1)?.Hip),
                AddToFlattenedList(human.Pose.Legs.ElementAtOrDefault(1)?.Knee),
                AddToFlattenedList(human.Pose.Legs.ElementAtOrDefault(1)?.Foot?.Ankle),
                AddToFlattenedList(human.Pose.Legs.ElementAtOrDefault(1)?.Foot?.Heel),
                AddToFlattenedList(human.Pose.Legs.ElementAtOrDefault(1)?.Foot?.Toes),

                AddToFlattenedList(human.Pose.Legs.ElementAtOrDefault(0)?.Hip),
                AddToFlattenedList(human.Pose.Legs.ElementAtOrDefault(0)?.Knee),
                AddToFlattenedList(human.Pose.Legs.ElementAtOrDefault(0)?.Foot?.Ankle),
                AddToFlattenedList(human.Pose.Legs.ElementAtOrDefault(0)?.Foot?.Heel),
                AddToFlattenedList(human.Pose.Legs.ElementAtOrDefault(0)?.Foot?.Toes),
            };
            human.FaceFlattened = new double[][]
            {
                // Face
                AddToFlattenedList(human.Face.Nose.Tip),

                AddToFlattenedList(human.Face.Head.ChinBottom),
                AddToFlattenedList(human.Face.Head.Outer.ElementAtOrDefault(27)),
                AddToFlattenedList(human.Face.Head.Outer.ElementAtOrDefault(8)),

                AddToFlattenedList(human.Face.Eyes.ElementAtOrDefault(0)?.Pupil),
                AddToFlattenedList(human.Face.Eyes.ElementAtOrDefault(0)?.Brow.FirstOrDefault()),
                AddToFlattenedList(human.Face.Eyes.ElementAtOrDefault(0)?.Brow.LastOrDefault()),

                AddToFlattenedList(human.Face.Eyes.ElementAtOrDefault(1)?.Pupil),
                AddToFlattenedList(human.Face.Eyes.ElementAtOrDefault(1)?.Brow.FirstOrDefault()),
                AddToFlattenedList(human.Face.Eyes.ElementAtOrDefault(1)?.Brow.LastOrDefault()),

                AddToFlattenedList(human.Face.Mouth.Left),
                AddToFlattenedList(human.Face.Mouth.Right),
                AddToFlattenedList(human.Face.Mouth.UpperLipBottom),
                AddToFlattenedList(human.Face.Mouth.LowerLipTop),
            };
            human.LeftHandFlattened = new double[][]
            {
                // Hands
                // Left Hand
                AddToFlattenedList(human.Hands.ElementAtOrDefault(0)?.Wrist),

                AddToFlattenedList(human.Hands.ElementAtOrDefault(0)?.Thumb.Tip),

                AddToFlattenedList(human.Hands.ElementAtOrDefault(0)?.Index.Tip),

                AddToFlattenedList(human.Hands.ElementAtOrDefault(0)?.Middle.Tip),

                AddToFlattenedList(human.Hands.ElementAtOrDefault(0)?.Ring.Tip),

                AddToFlattenedList(human.Hands.ElementAtOrDefault(0)?.Pinky.Tip),
            };
            human.RightHandFlattened = new double[][]
            {
                // Right Hand
                AddToFlattenedList(human.Hands.ElementAtOrDefault(1)?.Wrist),

                AddToFlattenedList(human.Hands.ElementAtOrDefault(1)?.Thumb.Tip),

                AddToFlattenedList(human.Hands.ElementAtOrDefault(1)?.Index.Tip),

                AddToFlattenedList(human.Hands.ElementAtOrDefault(1)?.Middle.Tip),

                AddToFlattenedList(human.Hands.ElementAtOrDefault(1)?.Ring.Tip),

                AddToFlattenedList(human.Hands.ElementAtOrDefault(1)?.Pinky.Tip),
            };

            humans.Add(human);

            return humans;
        }

        private static Pose CreatePose(dynamic poseResults)
        {
            if (poseResults == null) return new Pose();

            Pose pose = new Pose();

            Arm leftArm = new Arm()
            {
                Shoulder = CoordsToVector3(poseResults.left_shoulder),
                Elbow = CoordsToVector3(poseResults.left_elbow),
                Hand = CoordsToVector3(poseResults.left_wrist),
            };
            Arm rightArm = new Arm()
            {
                Shoulder = CoordsToVector3(poseResults.right_shoulder),
                Elbow = CoordsToVector3(poseResults.right_elbow),
                Hand = CoordsToVector3(poseResults.right_wrist),
            };
            pose.Arms = new Arm[2] { leftArm, rightArm };

            Leg leftLeg = new Leg()
            {
                Hip = CoordsToVector3(poseResults.left_hip),
                Knee = CoordsToVector3(poseResults.left_knee),

                Foot = new Foot()
                {
                    Ankle = CoordsToVector3(poseResults.left_ankle),
                    Heel = CoordsToVector3(poseResults.left_heel),
                    Toes = CoordsToVector3(poseResults.left_foot_index),
                },
            };
            Leg rightLeg = new Leg()
            {
                Hip = CoordsToVector3(poseResults.right_hip),
                Knee = CoordsToVector3(poseResults.right_knee),

                Foot = new Foot()
                {
                    Ankle = CoordsToVector3(poseResults.right_ankle),
                    Heel = CoordsToVector3(poseResults.right_heel),
                    Toes = CoordsToVector3(poseResults.right_foot_index),
                },
            };
            pose.Legs = new Leg[2] { leftLeg, rightLeg };

            return pose;
        }

        private static Hand CreateHand(dynamic handResults)
        {
            if (handResults == null) return new Hand();

            Hand hand = new Hand();

            hand.Wrist = CoordsToVector3(handResults.wrist);

            hand.Thumb = new Finger() { Mcp = CoordsToVector3(handResults.thumb_cmc), Pip = CoordsToVector3(handResults.thumb_mcp), Dip = CoordsToVector3(handResults.thumb_ip), Tip = CoordsToVector3(handResults.thumb_tip), };
            hand.Index = new Finger() { Mcp = CoordsToVector3(handResults.index_finger_mcp), Pip = CoordsToVector3(handResults.index_finger_pip), Dip = CoordsToVector3(handResults.index_finger_dip), Tip = CoordsToVector3(handResults.index_finger_tip), };
            hand.Middle = new Finger() { Mcp = CoordsToVector3(handResults.middle_finger_mcp), Pip = CoordsToVector3(handResults.middle_finger_pip), Dip = CoordsToVector3(handResults.middle_finger_dip), Tip = CoordsToVector3(handResults.middle_finger_tip), };
            hand.Ring = new Finger() { Mcp = CoordsToVector3(handResults.ring_finger_mcp), Pip = CoordsToVector3(handResults.ring_finger_pip), Dip = CoordsToVector3(handResults.ring_finger_dip), Tip = CoordsToVector3(handResults.ring_finger_tip), };
            hand.Pinky = new Finger() { Mcp = CoordsToVector3(handResults.pinky_mcp), Pip = CoordsToVector3(handResults.pinky_pip), Dip = CoordsToVector3(handResults.pinky_dip), Tip = CoordsToVector3(handResults.pinky_tip), };

            return hand;
        }

        private static Face CreateFace(dynamic faceResults)
        {
            if (faceResults == null) return new Face();

            Face face = new Face();

            List<Vector3> points = new List<Vector3>();

            Func<int, Vector3> useFacialLocation = (int index) =>
            {
                Vector3 element = points[index];

                return element;
            };

            foreach (dynamic coords in faceResults.list)
            {
                points.Add(CoordsToVector3(coords));
            }
            face.Eyes = new Eye[] { CreateLeftEye(useFacialLocation), CreateRightEye(useFacialLocation), };

            Vector3 headTopLeft = useFacialLocation(21);
            Vector3 headTopRight = useFacialLocation(251);
            Vector3 headMiddleLeft = useFacialLocation(234);
            Vector3 headMiddleRight = useFacialLocation(454);
            Vector3 headBottomLeft = useFacialLocation(172);
            Vector3 headBottomRight = useFacialLocation(397);
            Vector3 headBottomMiddle = useFacialLocation(152);
            face.Head = new Head()
            {
                Center = Vector3.Lerp(headMiddleLeft, headMiddleRight, 0.5f),
                ChinBottom = headBottomMiddle,

                Height = Vector3.Distance(Vector3.Lerp(headTopLeft, headTopRight, 0.5f), Vector3.Lerp(headBottomRight, headBottomRight, 0.5f)),
                Width = Vector3.Distance(headTopLeft, headTopRight),

                Outer = new Vector3[] {
                    useFacialLocation(10), useFacialLocation(338), useFacialLocation(297), useFacialLocation(284),
                    headTopRight, useFacialLocation(389), useFacialLocation(356),
                    headMiddleRight, useFacialLocation(323), useFacialLocation(361), useFacialLocation(288),
                    headBottomRight, useFacialLocation(365), useFacialLocation(379), useFacialLocation(378), useFacialLocation(400), useFacialLocation(377),
                    headBottomMiddle, useFacialLocation(148), useFacialLocation(176), useFacialLocation(149), useFacialLocation(150), useFacialLocation(136),
                    headBottomLeft, useFacialLocation(58), useFacialLocation(132), useFacialLocation(93),
                    headMiddleLeft, useFacialLocation(127), useFacialLocation(162),
                    headTopLeft, useFacialLocation(54), useFacialLocation(103), useFacialLocation(67), useFacialLocation(109),
                },
            };

            Vector3 noseTop = useFacialLocation(6);
            Vector3 noseTip = useFacialLocation(1);
            Vector3 noseBottom = useFacialLocation(2);
            Vector3 noseSideLeft = useFacialLocation(358);
            Vector3 noseSideRight = useFacialLocation(129);
            face.Nose = new Nose()
            {
                Tip = noseTip,

                Top = noseTop,
                Bottom = noseBottom,
                
                PointingArch = new Vector3[]
                {
                    noseSideRight, useFacialLocation(102), useFacialLocation(219), useFacialLocation(166), useFacialLocation(20), useFacialLocation(242), useFacialLocation(141),
                    useFacialLocation(94),
                    useFacialLocation(370), useFacialLocation(462), useFacialLocation(250), useFacialLocation(392), useFacialLocation(439), useFacialLocation(331), noseSideLeft,
                },

                Outer = new Vector3[] {
                    noseTop,
                    useFacialLocation(351), useFacialLocation(412), useFacialLocation(437), useFacialLocation(429),
                    noseSideLeft, useFacialLocation(460), useFacialLocation(326),
                    noseBottom,
                    useFacialLocation(97), useFacialLocation(240), noseSideRight,
                    useFacialLocation(209), useFacialLocation(217), useFacialLocation(188), useFacialLocation(122),
                },
            };

            Vector3 mouthLeft = useFacialLocation(61);
            Vector3 mouthRight = useFacialLocation(291);
            Vector3 mouthLipTop = useFacialLocation(0);
            Vector3 mouthInnerLipTop = useFacialLocation(13);
            Vector3 mouthInnerLipBottom = useFacialLocation(14);
            Vector3 mouthLipBottom = useFacialLocation(17);
            face.Mouth = new Mouth()
            {
                Left = mouthLeft,
                Right = mouthRight,

                UpperLipTop = mouthLipTop,
                UpperLipBottom = mouthInnerLipTop,
                LowerLipTop = mouthInnerLipBottom,
                LowerLipBottom = mouthLipBottom,

                Inner = new Vector3[] { mouthInnerLipTop, useFacialLocation(312), useFacialLocation(311), useFacialLocation(310), useFacialLocation(415), useFacialLocation(308), useFacialLocation(318), useFacialLocation(402), useFacialLocation(317), mouthInnerLipBottom, useFacialLocation(87), useFacialLocation(178), useFacialLocation(88), useFacialLocation(95), useFacialLocation(78), useFacialLocation(191), useFacialLocation(80), useFacialLocation(81), useFacialLocation(82), },
                Outer = new Vector3[] { mouthLipTop, useFacialLocation(267), useFacialLocation(269), useFacialLocation(270), useFacialLocation(409), mouthRight, useFacialLocation(375), useFacialLocation(321), useFacialLocation(405), useFacialLocation(314), mouthLipBottom, useFacialLocation(84), useFacialLocation(181), useFacialLocation(91), useFacialLocation(146), mouthLeft, useFacialLocation(185), useFacialLocation(40), useFacialLocation(39), useFacialLocation(37), },
            };

            face.LeftoverPoints = points.ToArray();

            return face;
        }

        private static Eye CreateLeftEye(Func<int, Vector3> useFacialLocation)
        {
            Vector3 leftSide = useFacialLocation(263);
            Vector3 rightSide = useFacialLocation(362);
            return new Eye()
            {
                Left = leftSide,
                Right = rightSide,
                Brow = new Vector3[] { useFacialLocation(276), useFacialLocation(283), useFacialLocation(282), useFacialLocation(295), useFacialLocation(285), },
                Pupil = Vector3.Lerp(leftSide, rightSide, 0.5f),

                Outer = new Vector3[] {
                    rightSide, useFacialLocation(398), useFacialLocation(384), useFacialLocation(385), useFacialLocation(386), useFacialLocation(387), useFacialLocation(388), useFacialLocation(466),
                    leftSide, useFacialLocation(249), useFacialLocation(390), useFacialLocation(373), useFacialLocation(374), useFacialLocation(380), useFacialLocation(381), useFacialLocation(382),
                },
            };
        }

        private static Eye CreateRightEye(Func<int, Vector3> useFacialLocation)
        {
            Vector3 leftSide = useFacialLocation(133);
            Vector3 rightSide = useFacialLocation(33);
            return new Eye()
            {
                Left = leftSide,
                Right = rightSide,
                Brow = new Vector3[] { useFacialLocation(46), useFacialLocation(53), useFacialLocation(52), useFacialLocation(65), useFacialLocation(55), },
                Pupil = Vector3.Lerp(leftSide, rightSide, 0.5f),

                Outer = new Vector3[] {
                    rightSide, useFacialLocation(246), useFacialLocation(161), useFacialLocation(160), useFacialLocation(159), useFacialLocation(158), useFacialLocation(157), useFacialLocation(173),
                    leftSide, useFacialLocation(155), useFacialLocation(154), useFacialLocation(153), useFacialLocation(145), useFacialLocation(144), useFacialLocation(163), useFacialLocation(7),
                },
            };
        }

        private static Vector3 CoordsToVector3(dynamic coords)
        {
            Vector3 vector3;
            if (coords == null) vector3 = new Vector3(0.0f, 0.0f, 0.0f);
            else vector3 = new Vector3(RelativeLocationToPixelLocation_X((float)coords.x), RelativeLocationToPixelLocation_Y((float)coords.y), RelativeLocationToPixelLocation_Z((float)coords.z));

            return vector3;
        }

        private static double[] AddToFlattenedList(Vector3? vector3)
        {
            return new double[3] { vector3?.X ?? 0.0, vector3?.Y ?? 0.0, vector3?.Z ?? 0.0 };
        }

        private static float RelativeLocationToPixelLocation_X(float x)
        {
            return x * _width;
        }

        private static float RelativeLocationToPixelLocation_Y(float y)
        {
            return y * _height;
        }

        private static float RelativeLocationToPixelLocation_Z(float z) // z may be relative to the individual object it is contained in (face, pose, left hand, right hand)
        {
            return z;
        }
    }
}
