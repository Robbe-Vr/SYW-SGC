using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace StudioControlGestureRecognition.Exchange.Objects
{
    public class VideoInputDevice
    {
        public VideoInputDevice()
        {

        }

        public VideoInputDevice(string name, Guid classId, string devicePath)
        {
            Name = name;
            ClassID = classId;
            DevicePath = devicePath;
        }

        public string Name { get; set; } = string.Empty;
        public Guid? ClassID { get; set; }
        public string? DevicePath { get; set; }
    }
}
