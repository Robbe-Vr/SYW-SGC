using StudioControlGestureRecognition.Exchange.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudioControlGestureRecognition.Exchange.Storage
{
    public class Preferences
    {
        public Preferences()
        {
            DefaultVideoCamera = new VideoInputDevice();
            FPS = 15;
            VideoWidth = 300;
            VideoHeight = 300;
        }

        public VideoInputDevice DefaultVideoCamera { get; set; }
        public int FPS { get; set; }
        public int VideoWidth { get; set; }
        public int VideoHeight { get; set; }
    }
}
