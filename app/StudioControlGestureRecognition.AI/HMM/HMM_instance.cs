using Accord.IO;
using Accord.Statistics.Distributions.Fitting;
using Accord.Statistics.Distributions.Multivariate;
using Accord.Statistics.Models.Markov;
using Accord.Statistics.Models.Markov.Learning;
using Accord.Statistics.Models.Markov.Topology;
using CsvHelper.TypeConversion;
using StudioControlGestureRecognition.Exchange.Gesture_Database;
using StudioControlGestureRecognition.Exchange.Objects;
using System.Diagnostics;
using System.Drawing;

namespace StudioControlGestureRecognition.AI.HMM
{
    internal class HMM_instance : IAIModel
    {
        private GestureClass[] _classes;
        private HiddenMarkovClassifier<MultivariateNormalDistribution, double[]>? _classifier;

        public bool LoadedFromFile { get; private set; }
        public bool IsTrained { get; private set; }

        private CancellationTokenSource? _cancellationTokenSource;

        private readonly string _modelName;

        public HMM_instance(string modelName, GestureClass[] classes, string filePath, bool forceTrainModel, ILabeledDataSet[]? trainingDataSets = null, bool enableTraining = true)
        {
            _modelName = modelName;
            _classes = classes;

            if ((forceTrainModel || !LoadFromFile(filePath)) && enableTraining)
            {
                if (trainingDataSets == null) { Trace.WriteLine("Unable to train model without training datasets! Please use the parameter 'trainingDataSets'"); return; }

                Train(trainingDataSets, initial: true).ContinueWith((task) =>
                {
                    if (task.IsCanceled)
                    {
                        Trace.WriteLine("Training took too long and was cancelled!");
                    }
                    else if (task.Exception != null)
                    {
                        Trace.WriteLine($"Training model '{_modelName}' faced an exception! Error: {task.Exception.Message}\nSource: {task.Exception.Source}\nStackTrace:\n{task.Exception.StackTrace}\n");
                    }
                    else
                    {
                        IsTrained = true;
                    }
                });
            }
        }

        private bool LoadFromFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath) && new FileInfo(filePath).Length > 0) _classifier = Serializer.Load<HiddenMarkovClassifier<MultivariateNormalDistribution, double[]>>(filePath);
                else throw new FileNotFoundException($"An existing trained model for this model '{_modelName}' was not found!");

                Trace.WriteLine($"Model '{_modelName}' loaded from file!");
                return LoadedFromFile = IsTrained = true;
            }
            catch (Exception e)
            {
                Trace.WriteLine($"Failed to load model '{_modelName}' from file!\nError: {e.Message}\nSource: {e.Source}\nStackTrace:\n{e.StackTrace}");

                return false;
            }
        }

        public async Task Train(ILabeledDataSet[] trainingDataSets, GestureClass[]? classes = null, bool initial = false)
        {
            _cancellationTokenSource = new CancellationTokenSource();

            _cancellationTokenSource.CancelAfter(initial ? TimeSpan.FromMinutes(30) : TimeSpan.FromHours(6));

            await Task.Run(() =>
            {                                                                                                                                                                                                                                                               
                if (classes != null) _classes = classes;

                if (trainingDataSets.Length == 0) return;

                Trace.WriteLine($"Training for model '{trainingDataSets[0].GestureClass.Group}' has started!");

                DateTime trainingStartedTime = DateTime.Now;

                MultivariateNormalDistribution initialDensity = new MultivariateNormalDistribution(trainingDataSets[0].Data[0].Length);

                HiddenMarkovClassifier<MultivariateNormalDistribution, double[]> classifier = new HiddenMarkovClassifier<MultivariateNormalDistribution, double[]>(classes: _classes.Length, topology: new Forward(trainingDataSets[0].Data.Length), initialDensity, names: _classes.Select(c => c.Label).ToArray());

                HiddenMarkovClassifierLearning<MultivariateNormalDistribution, double[]> teacher = new HiddenMarkovClassifierLearning<MultivariateNormalDistribution, double[]>(classifier);

                teacher.Learner = (modelIndex) => new BaumWelchLearning<MultivariateNormalDistribution, double[]>(_classifier?.Models.FirstOrDefault(m => m.Tag as string == _classes[modelIndex].Label) ?? classifier.Models[modelIndex])
                {
                    Tolerance = 0.0001,
                    MaxIterations = 100_000,

                    FittingOptions = new NormalOptions()
                    {
                        Diagonal = true,
                        Regularization = 1e-6
                    },
                    
                };

                double[][][] trainingSequences = trainingDataSets.Select(s => s.Data.Select(d => d.Select(value => value as double? ?? 0.0).ToArray()).ToArray()).ToArray();
                int[] labelsPerTrainingSequence = trainingDataSets.Select(s => Array.FindIndex(_classes, 0, c => c.Label == s.GestureClass.Label)).ToArray();

                _classifier = teacher.Learn(trainingSequences, labelsPerTrainingSequence);

                TimeSpan duration = DateTime.Now - trainingStartedTime;

                int testIndex = new Random().Next(0, trainingSequences.Length - 1);

                Trace.WriteLine($"Training Completed with resulting accuracy: {_classifier.Score(trainingSequences[testIndex], labelsPerTrainingSequence[testIndex])}\nDuration: {Math.Floor(duration.TotalHours)} hours, {duration.Minutes} minutes and {duration.Seconds} seconds (+{duration.Milliseconds}ms)!");

                IsTrained = true;
            }, _cancellationTokenSource.Token);
        }

        public (int index, string? label) Predict(double[][] data)
        {
            if (_classifier == null) return (0, null);

            double[] result = new double[_classes.Length];
            _classifier.Probabilities(data, out int decision, result);

            if (decision == 0)
                return (0, null);

            else return (decision, _classes[decision].Label);
        }

        public void SaveModelState(FileStream stream)
        {
            if (_classifier == null || _classifier.NumberOfClasses < 1 || _classifier.Models.Length < 1) return;

            Serializer.Save(_classifier, stream);
        }
    }
}