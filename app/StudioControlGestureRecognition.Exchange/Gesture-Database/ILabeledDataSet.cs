using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudioControlGestureRecognition.Exchange.Gesture_Database
{
    public interface ILabeledDataSet
    {
        GestureClass GestureClass { get; }
        double[][] Data { get; }
    }
}
