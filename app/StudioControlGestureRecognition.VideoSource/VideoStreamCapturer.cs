using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectShowLib;
using StudioControlGestureRecognition.Exchange.Objects;
using OpenCvSharp;
using System.Timers;
using OpenCvSharp.Extensions;
using System.Diagnostics;
using DirectShowLib.BDA;

namespace StudioControlGestureRecognition.AI
{
    public class VideoStreamCapturer : IDisposable
    {
        private int _width = 300;
        public int Width { get { return _width; } set { if (_width > 50 && _width < 1920) _width = value; } }
        private int _height = 300;
        public int Height { get { return _height; } set { if (_height > 50 && _width < 1920) _width = value; } }

        private int _fps = 20;
        public int FPS { get { return _fps; } }

        private int deviceIndex = 0;

        private VideoCapture? capture;

        public VideoStreamCapturer(VideoInputDevice defaultVideoInputDevice, int fps, int width, int height)
        {
            _fps = fps;
            _width = width;
            _height = height;

            SetDevice(defaultVideoInputDevice.Name, initial: true);
        }

        public bool SetDevice(string videoInputDeviceName, bool initial = false)
        {
            DsDevice[] devices = GetDevices();

            for (int i = 0; i < devices.Length; i++)
            {
                if (devices[i].Name == videoInputDeviceName)
                {
                    deviceIndex = i;
                    InitVideoCapture();
                    return true;
                }
            }

            Trace.WriteLine($"Video Input Device '{videoInputDeviceName}' was not found!");

            if (initial && deviceIndex < 1)
            {
                deviceIndex = 0;
                return true;
            }

            return false;
        }

        public bool UpdateFPS(int fps)
        {
            if (fps > 5 && fps < 60)
            {
                _fps = fps;

                InitVideoCapture();
                return true;
            }

            return false;
        }

        private void InitVideoCapture()
        {
            capture?.Release();
            capture?.Dispose();

            capture = new VideoCapture(deviceIndex);
            capture.Fps = (1000 / _fps); // AI will process an image every 100 ms
            capture.FrameWidth = 1920;
            capture.FrameHeight = 1080;
            capture.Open(deviceIndex);
        }

        private DsDevice[] GetDevices()
        {
            return DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
        }

        public IEnumerable<VideoInputDevice> ListVideoInputDevices()
        {
            return GetDevices().Select(dev => new VideoInputDevice() { Name = dev.Name, ClassID = dev.ClassID, DevicePath = dev.DevicePath }).ToArray();
        }

        public Mat? GetNextFrame()
        {
            if (capture != null)
                using (Mat mat = new())
                    if (capture.Read(mat))
                        using (Mat resizedMat = mat.Resize(new OpenCvSharp.Size(_width, _height)))
                            return resizedMat.Clone();

            return null;
        }

        public void Dispose()
        {
            capture?.Release();
            capture?.Dispose();
            capture = null;
        }

        public class NextFrameEventArgs : EventArgs
        {
            public NextFrameEventArgs(Mat frame)
            {
                Frame = frame;
            }

            public Mat Frame { get; set; }
        }
    }
}
