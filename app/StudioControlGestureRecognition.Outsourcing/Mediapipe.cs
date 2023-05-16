using System.Diagnostics;
using System.Drawing;
using System.Text;
using Newtonsoft.Json;
using StudioControlGestureRecognition.Exchange;
using StudioControlGestureRecognition.Exchange.Mediapipe;
using StudioControlGestureRecognition.Outsourcing.MediapipeUtils;

namespace StudioControlGestureRecognition.Outsourcing
{
    public static class Mediapipe
    {
        const string _clientName = "StudioGestureControl";
        const string _mediapipeMicroServiceUrl = "http://localhost:3999";

        private static OutsourcingHttpClient? _outsourcingClient;

        public static bool IsAvailable { get { return _outsourcingClient?.IsAvailable ?? false; } }

        public static void Init()
        {
            _outsourcingClient = new OutsourcingHttpClient(_clientName, _mediapipeMicroServiceUrl);
        }

        public static void CheckAvailability()
        {
            _outsourcingClient?.Connect();
        }

        public static void Stop()
        {
            _outsourcingClient?.Close();
        }

        public static void Restart()
        {
            _outsourcingClient = new OutsourcingHttpClient(_clientName, _mediapipeMicroServiceUrl);
        }

        public static async ValueTask<IEnumerable<Human>> Process(byte[] frame, long timestamp, int width, int height)
        {
            if (_outsourcingClient == null) throw new InvalidOperationException("Mediapipe outsourcing has not yet been Initialized! Please call 'Mediapipe.Init()' before processing any data.");

            try
            {
                object data = new
                {
                    img = Convert.ToBase64String(frame),
                    ts = timestamp,
                    w = width, h = height,
                };

                string content = await _outsourcingClient.ProcessAsync(data);

                dynamic? results = JsonConvert.DeserializeObject<dynamic>(content);

                if (results == null) throw new InvalidDataException("Unable to parse human detection results!");

                if (results.message != null)
                {
                    if (results.message.ToString().ToLower() == "client is not connected!")
                        await _outsourcingClient.ConnectAsync();

                    return Array.Empty<Human>();
                }

                return LandmarksFormatter.FormatResults(results, width, height);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);

                return Array.Empty<Human>();
            }
        }
    }
}