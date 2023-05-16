using OpenCvSharp;
using OpenCvSharp.Extensions;
using StudioControlGestureRecognition.Exchange.Gesture_Database;
using StudioControlGestureRecognition.Exchange.Mediapipe;
using StudioControlGestureRecognition.Exchange.Mediapipe.Face;
using StudioControlGestureRecognition.Exchange.Mediapipe.Hands;
using StudioControlGestureRecognition.Exchange.Mediapipe.Pose;
using StudioControlGestureRecognition.Exchange.Objects;
using StudioControlGestureRecognition.Outsourcing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Numerics;
using System.Runtime.CompilerServices;
using Point = OpenCvSharp.Point;

namespace StudioControlGestureRecognition.ImageProcessing
{
    public class ImageProcessor
    {
        private Mat? _blackTemplate;

        private Mat? _prevMat;

        private (int width, int height) _shape;

        private IDictionary<string, Mat> _errorMats = new Dictionary<string, Mat>();

        public ImageProcessor(int width, int height)
        {
            _shape = (width, height);
            CreateTemplates();

            Outsourcing.Mediapipe.Init();
        }

        private void CreateTemplates()
        {
            if (_blackTemplate != null)
            {
                _blackTemplate.Release();
                _blackTemplate.Dispose();
            }
            _blackTemplate = CreateBlackTemplate();

            Mat mediaPipeErrorMat = _blackTemplate.Clone();
            mediaPipeErrorMat.PutText("Mediapipe", new Point(_blackTemplate.Width / 5, (_blackTemplate.Height / 4) + 3), HersheyFonts.HersheySimplex, 1, Scalar.White, thickness: 2);
            mediaPipeErrorMat.PutText("Outsourcing", new Point(_blackTemplate.Width / 5, (_blackTemplate.Height / 4) * 2 + 3), HersheyFonts.HersheySimplex, 1, Scalar.White, thickness: 2);
            mediaPipeErrorMat.PutText("Unavailable", new Point(_blackTemplate.Width / 5, (_blackTemplate.Height / 4) * 3 + 3), HersheyFonts.HersheySimplex, 1, Scalar.White, thickness: 2);
            if (_errorMats.ContainsKey("mediapipe"))
            {
                _errorMats["mediapipe"].Release();
                _errorMats["mediapipe"].Dispose();
                _errorMats["mediapipe"] = mediaPipeErrorMat;
            }
            else _errorMats.Add("mediapipe", mediaPipeErrorMat);
        }

        public void SetVideoShape(int width, int height)
        {
            _shape = (width, height);

            CreateTemplates();
        }
        
        public void ResetPrevMat()
        {
            _prevMat?.Release();
            _prevMat?.Dispose();
            _prevMat = null;
        }

        public void Dispose()
        {
            ResetPrevMat();

            _blackTemplate?.Release();
            _blackTemplate?.Dispose();

            foreach (KeyValuePair<string, Mat> pair in _errorMats)
            {
                pair.Value.Release();
                pair.Value.Dispose();
            }
            _errorMats.Clear();

            Outsourcing.Mediapipe.Stop();
        }

        public async ValueTask<(Mat, double[][], IEnumerable<Human>)> ProcessImage(Mat mat, VideoDisplayMode displayMode)
        {
            long timestamp = DateTime.UtcNow.Ticks;

            Mat processedImage = mat.Clone();

            List<double[]> motionRects = new List<double[]>();
            List<Human> humans = new List<Human>();

            using (Mat grayImage = new Mat())
            {
                Cv2.CvtColor(mat, grayImage, ColorConversionCodes.BGRA2GRAY);

                try
                {
                    Mat detectedMotion;

                    (Point[][] motion, detectedMotion) = DetectMotion(grayImage);

                    if (displayMode == VideoDisplayMode.DetectedMotionOnly)
                    {
                        processedImage = detectedMotion;
                    }
                    else
                    {
                        detectedMotion.Release();
                        detectedMotion.Dispose();
                    }

                    if (displayMode == VideoDisplayMode.DetectedMotion)
                    {
                        foreach (Point[] contour in motion)
                        {
                            Rect rect = Cv2.BoundingRect(contour);

                            motionRects.Add(new double[4] { rect.X, rect.Y, rect.Width, rect.Height });

                            processedImage.Rectangle(rect, Scalar.FromRgb(45, 212, 255), thickness: 2);
                        }
                    }
                }
                catch (Exception e)
                {
                    Trace.WriteLine($"Failed to detect motion!\nError: {e.Message}\nSource: {e.Source}\nStackTrace:\n{e.StackTrace}");
                }

                if (Outsourcing.Mediapipe.IsAvailable)
                {
                    try
                    {
                        humans.AddRange(await DetectHumans(grayImage, timestamp));

                        if (displayMode == VideoDisplayMode.DetectedHumans || displayMode == VideoDisplayMode.DetectedHumansOnly)
                        {
                            processedImage = displayMode == VideoDisplayMode.DetectedHumans ? processedImage : GetBlackTemplate().Clone();

                            Func<float, (int r, int g, int b)?, Scalar> shiftColor = (z, optionalColor) =>
                            {
                                (int r, int g, int b) color = optionalColor ?? (r: 42, g: 225, b: 255);

                                return Scalar.FromRgb((int)Math.Round((color.r / 2) + ((color.r / 2.5) * ((z / 200) * -1))), (int)Math.Round((color.g / 2) + ((color.g / 2.5) * ((z / 200) * -1))), (int)Math.Round((color.b / 2) + ((color.b / 2.5) * ((z / 200) * -1))));
                            };

                            Action<Vector3, int, (int r, int g, int b)?> drawCircle = (coords, radius, color) =>
                            {
                                int x = (int)Math.Round(coords.X);
                                int y = (int)Math.Round(coords.Y);
                                processedImage.Circle(x, y, radius, shiftColor(coords.Z, color), radius);
                                processedImage.Circle(x, y, radius, shiftColor(coords.Z, color), radius);
                            };

                            Action<Vector3, Vector3, int, (int r, int g, int b)?> drawLine = (coords1, coords2, thickness, color) =>
                            {
                                int x1 = (int)Math.Round(coords1.X);
                                int y1 = (int)Math.Round(coords1.Y);
                                int x2 = (int)Math.Round(coords2.X);
                                int y2 = (int)Math.Round(coords2.Y);
                                processedImage.Line(x1, y1, x2, y2, shiftColor(coords1.Z, color), thickness);
                                processedImage.Line(x1, y1, x2, y2, shiftColor(coords1.Z, color), thickness);
                            };

                            Action<Vector3[], int, (int r, int g, int b)?> drawMultiPointLine = (coordsList, thickness, color) =>
                            {
                                for (int i = 1; i < coordsList.Length; i++)
                                {
                                    drawLine(coordsList[i], coordsList[i - 1], thickness, color);
                                }
                            };


                            Action<Vector3[], int, (int r, int g, int b)?> drawMultiPointShape = (coordsList, thickness, color) =>
                            {
                                for (int i = 0; i < coordsList.Length; i++)
                                {
                                    if (i == 0)
                                        drawLine(coordsList[i], coordsList[coordsList.Length - 1], thickness, color);
                                    else drawLine(coordsList[i], coordsList[i - 1], thickness, color);
                                }
                            };

                            foreach (Human human in humans)
                            {
                                if (human.Pose != null)
                                {
                                    (int r, int g, int b) poseColor = (24, 255, 68);

                                    foreach (Arm? arm in human.Pose.Arms ?? Array.Empty<Arm?>())
                                    {
                                        if (arm != null)
                                        {
                                            drawCircle(arm.Shoulder, 2, poseColor);
                                            drawCircle(arm.Elbow, 2, poseColor);
                                            if (arm.Hand != Vector3.Zero)
                                                drawCircle(arm.Hand, 2, poseColor);

                                            drawLine(arm.Shoulder, arm.Elbow, 1, poseColor);
                                            drawLine(arm.Elbow, arm.Hand, 1, poseColor);
                                        }
                                    }

                                    if (human.Pose.Arms?.Length > 0 && human.Pose.Arms[0] != null && human.Pose.Arms[1] != null &&
                                        human.Pose.Arms[0]?.Shoulder != null && human.Pose.Arms[1]?.Shoulder != null)
                                    {
                                        Vector3? leftShoulder = human.Pose.Arms[0]?.Shoulder;
                                        Vector3? rightShoulder = human.Pose.Arms[1]?.Shoulder;

                                        if (leftShoulder != null && rightShoulder != null)
                                        {
                                            drawLine(leftShoulder.Value, rightShoulder.Value, 2, poseColor);

                                            Vector3 neck = Vector3.Lerp(leftShoulder.Value, rightShoulder.Value, 0.5f);

                                            drawCircle(neck, 3, poseColor);

                                            if (human.Face?.Head?.ChinBottom != null && human.Face.Head.ChinBottom != Vector3.Zero &&
                                                human.Face.Head?.Center != null && human.Face.Head.Center != Vector3.Zero)
                                            {
                                                Vector3 neckTop = Vector3.Lerp(neck, human.Face.Head.Center, 0.55f);
                                                drawLine(neck, neckTop, 2, poseColor);
                                                drawLine(neckTop, human.Face.Head.ChinBottom, 2, poseColor);
                                            }
                                        }
                                    }

                                    foreach (Leg? leg in human.Pose.Legs ?? Array.Empty<Leg?>())
                                    {
                                        if (leg != null)
                                        {
                                            drawCircle(leg.Hip, 2, poseColor);
                                            drawCircle(leg.Knee, 2, poseColor);

                                            drawLine(leg.Hip, leg.Knee, 1, poseColor);

                                            if (leg.Foot != null)
                                            {
                                                drawLine(leg.Knee, leg.Foot.Ankle, 1, poseColor);

                                                drawCircle(leg.Foot.Ankle, 2, poseColor);
                                                drawCircle(leg.Foot.Heel, 2, poseColor);
                                                drawCircle(leg.Foot.Toes, 2, poseColor);

                                                drawLine(leg.Foot.Ankle, leg.Foot.Heel, 2, poseColor);
                                                drawLine(leg.Foot.Heel, leg.Foot.Toes, 2, poseColor);
                                                drawLine(leg.Foot.Toes, leg.Foot.Ankle, 1, poseColor);
                                            }
                                        }
                                    }

                                    if (human.Pose.Legs?.Length > 0 && human.Pose.Legs[0] != null && human.Pose.Legs[1] != null)
                                    {
                                        Vector3? leftHip = human.Pose.Legs[0]?.Hip;
                                        Vector3? rightHip = human.Pose.Legs[1]?.Hip;

                                        if (leftHip != null && rightHip != null)
                                        {
                                            drawLine(leftHip.Value, rightHip.Value, 2, poseColor);

                                            if (human.Pose.Arms?[0] != null && human.Pose.Arms[1] != null &&
                                                human.Pose.Arms[0]?.Shoulder != null && human.Pose.Arms[1]?.Shoulder != null)
                                            {

                                                drawLine(leftHip.Value, human.Pose.Arms[0].Shoulder, 2, poseColor);
                                                drawLine(rightHip.Value, human.Pose.Arms[1].Shoulder, 2, poseColor);
                                            }
                                        }
                                    }
                                }

                                if (human.Face != null)
                                {
                                    (int r, int g, int b) faceColor = (24, 221, 255);

                                    foreach (Vector3 coord in human.Face.LeftoverPoints ?? Array.Empty<Vector3>())
                                    {
                                        drawCircle(coord, 1, (24, 81, 125));
                                    }

                                    if (human.Face.Head != null)
                                    {
                                        drawCircle(human.Face.Head.Center, 2, (31, 255, 68));
                                        drawMultiPointShape(human.Face.Head.Outer, 2, faceColor);
                                    }

                                    if (human.Face.Nose != null)
                                    {
                                        drawCircle(human.Face.Nose.Tip, 1, faceColor);

                                        drawLine(human.Face.Nose.Top, human.Face.Nose.Tip, 1, faceColor);
                                        drawLine(human.Face.Nose.Tip, human.Face.Nose.Bottom, 1, faceColor);

                                        drawMultiPointLine(human.Face.Nose.PointingArch, 1, faceColor);

                                        drawMultiPointShape(human.Face.Nose.Outer, 1, faceColor);
                                    }

                                    foreach (Eye eye in human.Face.Eyes)
                                    {
                                        if (eye != null)
                                        {
                                            drawCircle(eye.Pupil, 2, faceColor);

                                            drawMultiPointShape(eye.Outer, 1, faceColor);
                                            drawMultiPointLine(eye.Brow, 1, faceColor);
                                        }
                                    }

                                    if (human.Face.Mouth != null)
                                    {
                                        drawMultiPointShape(human.Face.Mouth.Outer, 2, faceColor);
                                        drawMultiPointShape(human.Face.Mouth.Inner, 2, faceColor);

                                        drawCircle(human.Face.Mouth.Left, 2, faceColor);
                                        drawCircle(human.Face.Mouth.Right, 2, faceColor);
                                    }
                                }

                                foreach (Hand? hand in human.Hands)
                                {
                                    (int r, int g, int b) handsColor = (255, 221, 24);

                                    if (hand != null)
                                    {
                                        drawCircle(hand.Wrist, 2, handsColor);

                                        if (hand.Thumb != null)
                                        {
                                            drawCircle(hand.Thumb.Mcp, 1, handsColor);
                                            drawCircle(hand.Thumb.Pip, 1, handsColor);
                                            drawCircle(hand.Thumb.Dip, 1, handsColor);
                                            drawCircle(hand.Thumb.Tip, 2, handsColor);

                                            drawLine(hand.Wrist, hand.Thumb.Mcp, 1, handsColor);
                                            drawLine(hand.Thumb.Mcp, hand.Thumb.Pip, 1, handsColor);
                                            drawLine(hand.Thumb.Pip, hand.Thumb.Dip, 1, handsColor);
                                            drawLine(hand.Thumb.Dip, hand.Thumb.Tip, 1, handsColor);

                                            if (hand.Index != null)
                                                drawLine(hand.Thumb.Pip, hand.Index.Mcp, 1, handsColor);
                                        }
                                        if (hand.Index != null)
                                        {
                                            drawCircle(hand.Index.Mcp, 1, handsColor);
                                            drawCircle(hand.Index.Pip, 1, handsColor);
                                            drawCircle(hand.Index.Dip, 1, handsColor);
                                            drawCircle(hand.Index.Tip, 2, handsColor);

                                            drawLine(hand.Wrist, hand.Index.Mcp, 1, handsColor);
                                            drawLine(hand.Index.Mcp, hand.Index.Pip, 1, handsColor);
                                            drawLine(hand.Index.Pip, hand.Index.Dip, 1, handsColor);
                                            drawLine(hand.Index.Dip, hand.Index.Tip, 1, handsColor);

                                            if (hand.Middle != null)
                                                drawLine(hand.Index.Mcp, hand.Middle.Mcp, 1, handsColor);
                                        }
                                        if (hand.Middle != null)
                                        {
                                            drawCircle(hand.Middle.Mcp, 1, handsColor);
                                            drawCircle(hand.Middle.Pip, 1, handsColor);
                                            drawCircle(hand.Middle.Dip, 1, handsColor);
                                            drawCircle(hand.Middle.Tip, 2, handsColor);

                                            drawLine(hand.Wrist, hand.Middle.Mcp, 1, handsColor);
                                            drawLine(hand.Middle.Mcp, hand.Middle.Pip, 1, handsColor);
                                            drawLine(hand.Middle.Pip, hand.Middle.Dip, 1, handsColor);
                                            drawLine(hand.Middle.Dip, hand.Middle.Tip, 1, handsColor);

                                            if (hand.Ring != null)
                                                drawLine(hand.Middle.Mcp, hand.Ring.Mcp, 1, handsColor);
                                        }
                                        if (hand.Ring != null)
                                        {
                                            drawCircle(hand.Ring.Mcp, 1, handsColor);
                                            drawCircle(hand.Ring.Pip, 1, handsColor);
                                            drawCircle(hand.Ring.Dip, 1, handsColor);
                                            drawCircle(hand.Ring.Tip, 2, handsColor);

                                            drawLine(hand.Wrist, hand.Ring.Mcp, 1, handsColor);
                                            drawLine(hand.Ring.Mcp, hand.Ring.Pip, 1, handsColor);
                                            drawLine(hand.Ring.Pip, hand.Ring.Dip, 1, handsColor);
                                            drawLine(hand.Ring.Dip, hand.Ring.Tip, 1, handsColor);

                                            if (hand.Pinky != null)
                                                drawLine(hand.Ring.Mcp, hand.Pinky.Mcp, 1, handsColor);
                                        }
                                        if (hand.Pinky != null)
                                        {
                                            drawCircle(hand.Pinky.Mcp, 1, handsColor);
                                            drawCircle(hand.Pinky.Pip, 1, handsColor);
                                            drawCircle(hand.Pinky.Dip, 1, handsColor);
                                            drawCircle(hand.Pinky.Tip, 2, handsColor);

                                            drawLine(hand.Wrist, hand.Pinky.Mcp, 1, handsColor);
                                            drawLine(hand.Pinky.Mcp, hand.Pinky.Pip, 1, handsColor);
                                            drawLine(hand.Pinky.Pip, hand.Pinky.Dip, 1, handsColor);
                                            drawLine(hand.Pinky.Dip, hand.Pinky.Tip, 1, handsColor);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Trace.WriteLine($"Failed to detect humans!\nError: {e.Message}\nSource: {e.Source}\nStackTrace:\n{e.StackTrace}");
                    }
                }
                else if (displayMode == VideoDisplayMode.DetectedHumans || displayMode == VideoDisplayMode.DetectedHumansOnly)
                {
                    processedImage = _errorMats["mediapipe"].Clone();
                }

                _prevMat = grayImage.Clone();
            }

            mat.Release();
            mat.Dispose();

            return (processedImage, motionRects.ToArray(), humans);
        }

        public Bitmap DisplayDataOnImage(double[][] data, DataSetGroup group, int width, int height)
        {
            using (Mat trainingDataSetMat = new Mat(width, height, MatType.CV_8UC3, Scalar.Black))
            {
                switch(group)
                {
                    default:
                        (int, int, int) color = (24, 221, 255);
                        Func<double, (int r, int g, int b)?, Scalar> shiftColor = (z, optionalColor) =>
                        {
                            (int r, int g, int b) color = optionalColor ?? (r: 42, g: 225, b: 255);

                            return Scalar.FromRgb((int)Math.Round((color.r / 2) + ((color.r / 2.5) * ((z / 200) * -1))), (int)Math.Round((color.g / 2) + ((color.g / 2.5) * ((z / 200) * -1))), (int)Math.Round((color.b / 2) + ((color.b / 2.5) * ((z / 200) * -1))));
                        };

                        Action<double[], int, (int r, int g, int b)?> drawCircle = (coords, radius, color) =>
                        {
                            int x = (int)Math.Round(coords[0]);
                            int y = (int)Math.Round(coords[1]);
                            trainingDataSetMat.Circle(x, y, radius, shiftColor(coords[2], color), radius);
                            trainingDataSetMat.Circle(x, y, radius, shiftColor(coords[2], color), radius);
                        };
                        foreach (double[] vector3 in data)
                        {
                            drawCircle(vector3, 2, color);
                        }
                        break;
                };

                return trainingDataSetMat.Clone().ToBitmap();
            }
        }

        private (Point[][], Mat) DetectMotion(Mat grayImage)
        {
            Point[][] frameContours;
            HierarchyIndex[] external;

            using (Mat absDiffImage = new Mat())
            using (Mat frameThresh = new Mat())
            {
                Cv2.Absdiff(grayImage, _prevMat ?? grayImage, absDiffImage);
                Cv2.Threshold(absDiffImage, frameThresh, 15, 255, ThresholdTypes.Binary);
                Cv2.FindContours(frameThresh, out frameContours, out external, RetrievalModes.List, ContourApproximationModes.ApproxSimple);

                return (frameContours, frameThresh.Clone());
            }
        }

        private async ValueTask<IEnumerable<Human>> DetectHumans(Mat image, long timestamp)
        {
            return await Outsourcing.Mediapipe.Process(image.ToBytes(), timestamp, image.Width, image.Height);
        }

        private Mat CreateBlackTemplate()
        {
            return new Mat(_shape.width, _shape.height, MatType.CV_8UC3, Scalar.Black);
        }

        public Mat GetBlackTemplate()
        {
            if (_blackTemplate?.IsDisposed == true) return _blackTemplate = CreateBlackTemplate();
            return _blackTemplate ??= CreateBlackTemplate();
        }

        public void CheckOutsourcingServices()
        {
            Outsourcing.Mediapipe.CheckAvailability();
        }
    }
}