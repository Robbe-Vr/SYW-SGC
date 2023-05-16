using StudioControlGestureRecognition.Exchange.Gesture_Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudioControlGestureRecognition.Exchange.Objects
{
    public interface IAIModel
    {
        Task Train(ILabeledDataSet[] trainingDataSets, GestureClass[]? classes = null, bool initial = false);
        (int index, string? label) Predict(double[][] data);
        void SaveModelState(FileStream stream);
    }
}
