using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudioControlGestureRecognition.Exchange.Gesture_Database
{
    public class GestureClass
    {
        public GestureClass() { }

        public GestureClass(string label, DataSetGroup group)
        {
            Label = label;
            Group = group;
        }
        public GestureClass(string label, DataSetGroup group, string action)
        {
            Label = label;
            Group = group;
            Action = action;
        }

        public string Label { get; set; } = string.Empty;
        public DataSetGroup Group { get; set; } = DataSetGroup.Gesture;

        public string? Action { get; set; }
    }
}
