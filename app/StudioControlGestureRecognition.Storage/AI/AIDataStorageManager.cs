using StudioControlGestureRecognition.Exchange.Gesture_Database;
using StudioControlGestureRecognition.Exchange.Objects;
using StudioControlGestureRecognition.Exchange.Storage;
using StudioControlGestureRecognition.Storage.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudioControlGestureRecognition.Storage.AI
{
    public class AIDataStorageManager
    {
        private static readonly string _aiDataStorageFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SYW-SC-GR", "AI");
        private static readonly string _aiTrainedModelsStorageFolderPath = Path.Combine(_aiDataStorageFolderPath, "Trained-Models");

        private readonly string _gestureClassesFilePath;
        private readonly string _trainingDataSetsFilePath;
        private readonly string _trainingGestureDataSetsFilePath;

        public AIDataStorageManager()
        {
            if (!Directory.Exists(_aiDataStorageFolderPath))
            {
                Directory.CreateDirectory(_aiDataStorageFolderPath);
            }

            if (!Directory.Exists(_aiTrainedModelsStorageFolderPath))
            {
                Directory.CreateDirectory(_aiTrainedModelsStorageFolderPath);
            }

            _gestureClassesFilePath = Path.Combine(_aiDataStorageFolderPath, "classes.csv");

            if (!File.Exists(_gestureClassesFilePath))
            {
                File.Create(_gestureClassesFilePath).Dispose();
            }

            _trainingDataSetsFilePath = Path.Combine(_aiDataStorageFolderPath, "training-datasets.json");

            if (!File.Exists(_trainingDataSetsFilePath))
            {
                File.Create(_trainingDataSetsFilePath).Dispose();
            }

            _trainingGestureDataSetsFilePath = Path.Combine(_aiDataStorageFolderPath, "gesture-training-datasets.json");

            if (!File.Exists(_trainingGestureDataSetsFilePath))
            {
                File.Create(_trainingGestureDataSetsFilePath).Dispose();
            }
        }

        public string GetPathForTrainedModel(DataSetGroup group)
        {
            return Path.Combine(_aiTrainedModelsStorageFolderPath, $"{group}.model");
        }

        public void SaveModelState(IAIModel model, DataSetGroup group)
        {
            using (FileStream stream = File.Create(GetPathForTrainedModel(group)))
            {
                model.SaveModelState(stream);
            }
        }

        public UnpreparedLabeledGestureSet[] LoadStaticGestureTrainingDataSets()
        {
            try
            {
                UnpreparedLabeledGestureSet[]? database = JsonUtils.Deserialize<UnpreparedLabeledGestureSet[]>(File.ReadAllText(_trainingDataSetsFilePath));

                return database ?? Array.Empty<UnpreparedLabeledGestureSet>();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);

                return Array.Empty<UnpreparedLabeledGestureSet>();
            }
        }

        public bool StoreStaticGestureTrainingDataSets(UnpreparedLabeledGestureSet[] classes)
        {
            try
            {
                File.WriteAllText(_trainingDataSetsFilePath, JsonUtils.Serialize(classes));

                return true;
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);

                return false;
            }
        }

        public LabeledGestureSet[] LoadGestureTrainingDataSets()
        {
            try
            {
                LabeledGestureSet[]? database = JsonUtils.Deserialize<LabeledGestureSet[]>(File.ReadAllText(_trainingGestureDataSetsFilePath));

                return database ?? Array.Empty<LabeledGestureSet>();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);

                return Array.Empty<LabeledGestureSet>();
            }
        }

        public bool StoreGestureTrainingDataSets(LabeledGestureSet[] classes)
        {
            try
            {
                File.WriteAllText(_trainingGestureDataSetsFilePath, JsonUtils.Serialize(classes));

                return true;
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);

                return false;
            }
        }

        public GestureClass[] LoadGestureClasses()
        {
            try
            {
                GestureClass[]? database = CsvUtils.Deserialize<GestureClass>(File.ReadAllText(_gestureClassesFilePath));

                return database ?? Array.Empty<GestureClass>();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);

                return Array.Empty<GestureClass>();
            }
        }

        public bool StoreGestureClasses(GestureClass[] classes)
        {
            try
            {
                File.WriteAllText(_gestureClassesFilePath, CsvUtils.Serialize(classes));

                return true;
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);

                return false;
            }
        }
    }
}
