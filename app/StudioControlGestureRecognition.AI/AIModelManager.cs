using Accord.Math;
using Accord.Statistics;
using StudioControlGestureRecognition.AI.HMM;
using StudioControlGestureRecognition.Exchange.Gesture_Database;
using StudioControlGestureRecognition.Exchange.Mediapipe;
using StudioControlGestureRecognition.Storage.AI;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace StudioControlGestureRecognition.AI
{
    public class AIModelManager
    {
        private GestureClass[] _gestureLabels = Array.Empty<GestureClass>();

        private readonly AIDataStorageManager _storage;

        private HMM_instance _classifier;
        private HMM_instance _poseClassifier;
        private HMM_instance _faceClassifier;
        private HMM_instance _leftHandClassifier;
        private HMM_instance _rightHandClassifier;

        public AIModelManager(bool forceTrainModels = true, bool disableTrainingModels = false)
        {
            _storage = new AIDataStorageManager();

            _gestureLabels = _storage.LoadGestureClasses();

            UnpreparedLabeledGestureSet[] trainingSets = _storage.LoadStaticGestureTrainingDataSets();
            LabeledGestureSet[] gestureTrainingSets = _storage.LoadGestureTrainingDataSets();

            _poseClassifier = new HMM_instance(DataSetGroup.Pose.ToString(), classes: _gestureLabels.Where(l => l.Group == DataSetGroup.Pose).ToArray(), trainingDataSets: trainingSets.Where(s => s.GestureClass.Group == DataSetGroup.Pose).Select(s => new LabeledStaticGestureSet() { Data = FormatData(SelectDataFromDataSet(s.Data, DataSetGroup.Pose)), GestureClass = s.GestureClass }).ToArray(), filePath: _storage.GetPathForTrainedModel(DataSetGroup.Face), forceTrainModel: forceTrainModels, enableTraining: !disableTrainingModels);
            if (!_poseClassifier.LoadedFromFile && _poseClassifier.IsTrained) _storage.SaveModelState(_poseClassifier, DataSetGroup.Pose);

            _faceClassifier = new HMM_instance(DataSetGroup.Face.ToString(), classes: _gestureLabels.Where(l => l.Group == DataSetGroup.Face).ToArray(), trainingDataSets: trainingSets.Where(s => s.GestureClass.Group == DataSetGroup.Face).Select(s => new LabeledStaticGestureSet() { Data = FormatData(SelectDataFromDataSet(s.Data, DataSetGroup.Face)), GestureClass = s.GestureClass }).ToArray(), filePath: _storage.GetPathForTrainedModel(DataSetGroup.Face), forceTrainModel: forceTrainModels, enableTraining: !disableTrainingModels);
            if (!_faceClassifier.LoadedFromFile && _faceClassifier.IsTrained) _storage.SaveModelState(_faceClassifier, DataSetGroup.Face);

            _leftHandClassifier = new HMM_instance(DataSetGroup.LeftHand.ToString(), classes: _gestureLabels.Where(l => l.Group == DataSetGroup.LeftHand).ToArray(), trainingDataSets: trainingSets.Where(s => s.GestureClass.Group == DataSetGroup.LeftHand).Select(s => new LabeledStaticGestureSet() { Data = FormatData(SelectDataFromDataSet(s.Data, DataSetGroup.LeftHand)), GestureClass = s.GestureClass }).ToArray(), filePath: _storage.GetPathForTrainedModel(DataSetGroup.LeftHand), forceTrainModel: forceTrainModels, enableTraining: !disableTrainingModels);
            if (!_leftHandClassifier.LoadedFromFile && _leftHandClassifier.IsTrained) _storage.SaveModelState(_leftHandClassifier, DataSetGroup.LeftHand);

            _rightHandClassifier = new HMM_instance(DataSetGroup.RightHand.ToString(), classes: _gestureLabels.Where(l => l.Group == DataSetGroup.RightHand).ToArray(), trainingDataSets: trainingSets.Where(s => s.GestureClass.Group == DataSetGroup.RightHand).Select(s => new LabeledStaticGestureSet() { Data = FormatData(SelectDataFromDataSet(s.Data, DataSetGroup.RightHand)), GestureClass = s.GestureClass }).ToArray(), filePath: _storage.GetPathForTrainedModel(DataSetGroup.RightHand), forceTrainModel: forceTrainModels, enableTraining: !disableTrainingModels);
            if (!_rightHandClassifier.LoadedFromFile && _rightHandClassifier.IsTrained) _storage.SaveModelState(_rightHandClassifier, DataSetGroup.RightHand);

            _classifier = new HMM_instance(DataSetGroup.Gesture.ToString(), classes: _gestureLabels.Where(l => l.Group == DataSetGroup.Gesture).ToArray(), trainingDataSets: gestureTrainingSets, filePath: _storage.GetPathForTrainedModel(DataSetGroup.Gesture), forceTrainModel: forceTrainModels, enableTraining: !disableTrainingModels);
            if (!_classifier.LoadedFromFile && _classifier.IsTrained) _storage.SaveModelState(_classifier, DataSetGroup.Gesture);
        }

        public async Task TrainModels()
        {
            UnpreparedLabeledGestureSet[] trainingSets = _storage.LoadStaticGestureTrainingDataSets();
            LabeledGestureSet[] gestureTrainingSets = _storage.LoadGestureTrainingDataSets();

            await _poseClassifier.Train(classes: _gestureLabels.Where(l => l.Group == DataSetGroup.Pose).ToArray(), trainingDataSets: trainingSets.Where(s => s.GestureClass.Group == DataSetGroup.Pose).Select(s => new LabeledStaticGestureSet() { Data = FormatData(SelectDataFromDataSet(s.Data, DataSetGroup.Pose)), GestureClass = s.GestureClass }).ToArray());
            _storage.SaveModelState(_poseClassifier, DataSetGroup.Pose);

            await _faceClassifier.Train(classes: _gestureLabels.Where(l => l.Group == DataSetGroup.Face).ToArray(), trainingDataSets: trainingSets.Where(s => s.GestureClass.Group == DataSetGroup.Face).Select(s => new LabeledStaticGestureSet() { Data = FormatData(SelectDataFromDataSet(s.Data, DataSetGroup.Face)), GestureClass = s.GestureClass }).ToArray());
            _storage.SaveModelState(_faceClassifier, DataSetGroup.Face);

            await _leftHandClassifier.Train(classes: _gestureLabels.Where(l => l.Group == DataSetGroup.LeftHand).ToArray(), trainingDataSets: trainingSets.Where(s => s.GestureClass.Group == DataSetGroup.LeftHand).Select(s => new LabeledStaticGestureSet() { Data = FormatData(SelectDataFromDataSet(s.Data, DataSetGroup.LeftHand)), GestureClass = s.GestureClass }).ToArray());
            _storage.SaveModelState(_leftHandClassifier, DataSetGroup.LeftHand);

            await _rightHandClassifier.Train(classes: _gestureLabels.Where(l => l.Group == DataSetGroup.RightHand).ToArray(), trainingDataSets: trainingSets.Where(s => s.GestureClass.Group == DataSetGroup.RightHand).Select(s => new LabeledStaticGestureSet() { Data = FormatData(SelectDataFromDataSet(s.Data, DataSetGroup.RightHand)), GestureClass = s.GestureClass }).ToArray());
            _storage.SaveModelState(_rightHandClassifier, DataSetGroup.RightHand);

            await _classifier.Train(classes: _gestureLabels.Where(l => l.Group == DataSetGroup.Gesture).ToArray(), trainingDataSets: gestureTrainingSets);
            _storage.SaveModelState(_classifier, DataSetGroup.Gesture);
        }
        
        public (int poseGestureIndex, string? poseGesture, int faceGestureIndex, string? faceGesture, int leftHandGestureIndex, string? leftHandGesture, int rightHandGestureIndex, string? rightHandGesture, int gestureIndex, string? gesture, double[][] updatedHistory) DetectGesture(GestureDetectionTrackingData data, double[][] gestureHistory)
        {
            (int poseStaticGestureIndex, string? poseStaticGesture) = _poseClassifier.Predict(FormatData(data.HumanPoseTracking));

            (int faceStaticGestureIndex, string? faceStaticGesture) = _faceClassifier.Predict(FormatData(data.HumanFaceTracking));

            (int leftHandStaticGestureIndex, string? leftHandStaticGesture) = _leftHandClassifier.Predict(FormatData(data.HumanLeftHandTracking));
            (int rightHandStaticGestureIndex, string? rightHandStaticGesture) = _rightHandClassifier.Predict(FormatData(data.HumanRightHandTracking));

            double[][] updatedHistory = gestureHistory.Append(new double[] { poseStaticGestureIndex, faceStaticGestureIndex, leftHandStaticGestureIndex, rightHandStaticGestureIndex }).Skip(1).ToArray();
            (int gestureIndex, string? gesture) = _classifier.Predict(updatedHistory);

            return (poseStaticGestureIndex, poseStaticGesture, faceStaticGestureIndex, faceStaticGesture, leftHandStaticGestureIndex, leftHandStaticGesture, rightHandStaticGestureIndex, rightHandStaticGesture, gestureIndex, gesture, updatedHistory);
        }

        private static double[][] FormatData(double[][] data)
        {
            double[] xVals = data.Select(d => d[0]).ToArray();
            double[] yVals = data.Select(d => d[1]).ToArray();

            double minX = 0.0;
            double maxX = 0.0;
            double minY = 0.0;
            double maxY = 0.0;

            foreach (double xVal in xVals)
            {
                if (xVal > maxX) maxX = xVal;
                else if (xVal < minX) minX = xVal;
            }
            foreach (double yVal in yVals)
            {
                if (yVal > maxY) maxY = yVal;
                else if (yVal < minY) minY = yVal;
            }

            double xDiff = maxX - minX;
            double yDiff = maxY - minY;

            foreach (double[] dataPoint in data)
            {
                dataPoint[0] = dataPoint[0] / xDiff;
                dataPoint[1] = dataPoint[1] / yDiff;
            }

            return data;
        }

        public string[] ListLabels(DataSetGroup group)
        {
            return (_gestureLabels = _storage.LoadGestureClasses()).Where(l => l.Group == group).Select(l => l.Label).ToArray();
        }

        public bool CreateNewGestureClass(string label, DataSetGroup group)
        {
            bool isHands = group == DataSetGroup.LeftHand || group == DataSetGroup.RightHand;

            _gestureLabels = _storage.LoadGestureClasses();

            if (_gestureLabels.Any(l => isHands ? l.Group == DataSetGroup.LeftHand || l.Group == DataSetGroup.RightHand : l.Group == group && l.Label == label)) return false;

            if (isHands)
            {

                GestureClass leftHandGestureClass = new GestureClass(label, DataSetGroup.LeftHand);
                GestureClass rightHandGestureClass = new GestureClass(label, DataSetGroup.RightHand);

                return _storage.StoreGestureClasses(_gestureLabels.Append(leftHandGestureClass).ToArray()) && _storage.StoreGestureClasses(_gestureLabels.Append(rightHandGestureClass).ToArray());
            }
            else
            {
                GestureClass gestureClass = new GestureClass(label, group);

                return _storage.StoreGestureClasses(_gestureLabels.Append(gestureClass).ToArray());
            }
        }

        public bool CreateTrainingData(GestureDetectionTrackingData dataSet, string label, DataSetGroup group)
        {
            GestureClass? gestureClass;
            if ((gestureClass = _gestureLabels.FirstOrDefault(l => l.Group == group && l.Label == label)) == null) return false;

            UnpreparedLabeledGestureSet trainingDataSet = new UnpreparedLabeledGestureSet()
            {
                GestureClass = gestureClass,
                Data = dataSet,
            };

            UnpreparedLabeledGestureSet[] trainingDataSets = _storage.LoadStaticGestureTrainingDataSets();

            return _storage.StoreStaticGestureTrainingDataSets(trainingDataSets.Append(trainingDataSet).OrderBy(x => x.GestureClass.Group).ThenBy(x => Array.FindIndex(_gestureLabels, 0, l => l.Group == x.GestureClass.Group && l.Label == x.GestureClass.Label)).ToArray());
        }

        public UnpreparedLabeledGestureSet[] LoadTrainingSets()
        {
            return _storage.LoadStaticGestureTrainingDataSets();
        }

        public bool CreateGestureTrainingData(GestureDetectionTrackingData[] staticGestureData, double[][] gestureHistoryDataSet, string label, DataSetGroup group)
        {
            GestureClass? gestureClass;
            if ((gestureClass = _gestureLabels.FirstOrDefault(l => l.Group == group && l.Label == label)) == null) return false;

            LabeledGestureSet trainingDataSet = new LabeledGestureSet()
            {
                GestureClass = gestureClass,
                Data = gestureHistoryDataSet,

                StaticGesturesData = staticGestureData
            };

            LabeledGestureSet[] trainingDataSets = _storage.LoadGestureTrainingDataSets();

            return _storage.StoreGestureTrainingDataSets(trainingDataSets.Append(trainingDataSet).OrderBy(x => x.GestureClass.Group).ThenBy(x => Array.FindIndex(_gestureLabels, 0, l => l.Group == x.GestureClass.Group && l.Label == x.GestureClass.Label)).ToArray());
        }

        public LabeledGestureSet[] LoadGetureTrainingSets()
        {
            return _storage.LoadGestureTrainingDataSets();
        }

        public static double[][] SelectDataFromDataSet(GestureDetectionTrackingData dataSet, DataSetGroup group)
        {
            return group switch
            {
                DataSetGroup.Motion => dataSet.MotionObjectsTracking,

                DataSetGroup.Pose => dataSet.HumanPoseTracking,

                DataSetGroup.Face => dataSet.HumanFaceTracking,

                DataSetGroup.LeftHand => dataSet.HumanLeftHandTracking,
                DataSetGroup.RightHand => dataSet.HumanRightHandTracking,

                _ => dataSet.HumanTracking,
            };
        }

        public bool DeleteLabel(string label, DataSetGroup dataSetGroup)
        {
            List<GestureClass> classes = _storage.LoadGestureClasses().ToList();

            classes.RemoveAll(c => c.Group == dataSetGroup && c.Label == label);

            List<UnpreparedLabeledGestureSet> trainingDataSets = _storage.LoadStaticGestureTrainingDataSets().ToList();

            trainingDataSets.RemoveAll(s => s.GestureClass.Group == dataSetGroup && s.GestureClass.Label == label);

            return _storage.StoreGestureClasses(classes.ToArray()) && _storage.StoreStaticGestureTrainingDataSets(trainingDataSets.ToArray());
        }

        public bool UpdateTrainingDataSet(int dataSetIndex, string newGestureLabel)
        {
            GestureClass? gestureClass = _storage.LoadGestureClasses().FirstOrDefault(c => c.Label == newGestureLabel);

            if (gestureClass == null) { return false; }

            UnpreparedLabeledGestureSet[] trainingDataSets = _storage.LoadStaticGestureTrainingDataSets();

            trainingDataSets.ElementAt(dataSetIndex).GestureClass = gestureClass;

            return _storage.StoreStaticGestureTrainingDataSets(trainingDataSets.OrderBy(x => x.GestureClass.Group).ThenBy(x => Array.FindIndex(_gestureLabels, 0, l => l.Group == x.GestureClass.Group && l.Label == x.GestureClass.Label)).ToArray());
        }

        public bool DeleteTrainingDataSet(int dataSetIndex)
        {
            UnpreparedLabeledGestureSet[] trainingDataSets = _storage.LoadStaticGestureTrainingDataSets().RemoveAt(dataSetIndex);

            return _storage.StoreStaticGestureTrainingDataSets(trainingDataSets);
        }
    }
}
