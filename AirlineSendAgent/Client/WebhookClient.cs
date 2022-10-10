using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using TravelAgentWeb.Dtos;

namespace AirlineSendAgent.Client
{
    public class WebhookClient : IWebhookClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public WebhookClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task SendWebhookNotification(FlightDetailChangePayloadDto payload)
        {
            var serializedPayload = JsonSerializer.Serialize(payload);
            var httpClient = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Post, payload.WebhookURI);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //request.Content = new StringContent(serializedPayload, encoding: Encoding.UTF8, "application/json");
            request.Content = new StringContent(serializedPayload);

            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            try
            {
                using (var response = await httpClient.SendAsync(request))
                {
                    Console.WriteLine("Successully Published and Sent Webhook Payload to Subscriber...");
                    response.EnsureSuccessStatusCode();
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Unsuccesful {ex.Message}");
            }



        }
    }

}