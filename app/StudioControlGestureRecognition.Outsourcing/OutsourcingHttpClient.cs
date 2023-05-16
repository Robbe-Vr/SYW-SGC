using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace StudioControlGestureRecognition.Outsourcing
{
    internal class OutsourcingHttpClient
    {
        private string _clientName;

        private Uri _connectUri;
        private Uri _disconnectUri;
        private Uri _processUri;
        private HttpClientHandler _handler;
        private HttpClient _client;

        private string? accessToken = null;

        public bool IsAvailable = false;

        internal OutsourcingHttpClient(string clientName, string baseUrl, string connectPath = "/connect", string disconnectPath = "/disconnect", string processPath = "/process")
        {
            _clientName = clientName;

            _connectUri = new Uri(baseUrl + connectPath);
            _disconnectUri = new Uri(baseUrl + disconnectPath);
            _processUri = new Uri(baseUrl + processPath);
            _handler = new HttpClientHandler() { MaxConnectionsPerServer = 10 };
            _client = new HttpClient(_handler);

            Connect();
        }

        internal bool Connect()
        {
            try
            {
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, _connectUri);

                message.Content = JsonContent.Create(new { clientName = _clientName });

                HttpResponseMessage response = _client.Send(message);

                response.EnsureSuccessStatusCode();

                StreamReader reader = new StreamReader(response.Content.ReadAsStream());

                string content = reader.ReadToEnd();

                dynamic? result = JsonConvert.DeserializeObject<dynamic>(content);

                accessToken = result?.token;

                return IsAvailable = response.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                Trace.WriteLine($"Failed to connect to outsourcing service! Reason: {e.Message}");

                return IsAvailable = false;
            }
        }

        internal async ValueTask<bool> ConnectAsync()
        {
            try
            {
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, _connectUri);

                message.Content = JsonContent.Create(new { clientName = _clientName });

                HttpResponseMessage response = await _client.SendAsync(message);

                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();

                return IsAvailable = response.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                Trace.WriteLine($"Failed to connect to outsourcing service! Reason: {e.Message}");

                return IsAvailable = false;
            }
        }

        internal string Process(object data)
        {
            JsonContent json = JsonContent.Create(new { data, accessToken });

            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, _processUri);

            message.Content = json;

            HttpResponseMessage response = _client.Send(message);

            response.EnsureSuccessStatusCode();

            StreamReader reader = new StreamReader(response.Content.ReadAsStream());

            string content = reader.ReadToEnd();

            return content;
        }

        internal async ValueTask<string> ProcessAsync(object data)
        {
            JsonContent json = JsonContent.Create(new { data, accessToken });

            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, _processUri);

            message.Content = json;

            HttpResponseMessage response = await _client.SendAsync(message);

            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();

            return content;
        }

        internal void Close()
        {
            try
            {
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, _disconnectUri);

                message.Content = JsonContent.Create(new { clientName = _clientName });

                HttpResponseMessage response = _client.Send(message);

                IsAvailable = response.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                Trace.WriteLine($"Failed to disconnect from outsourcing service! Reason: {e.Message}");

                IsAvailable = false;
            }

            _client.CancelPendingRequests();
            _client.Dispose();
        }
    }
}
