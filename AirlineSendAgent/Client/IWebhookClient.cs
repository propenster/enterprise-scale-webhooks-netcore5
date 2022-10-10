using TravelAgentWeb.Dtos;

namespace AirlineSendAgent.Client
{

    public interface IWebhookClient
    {
        Task SendWebhookNotification(FlightDetailChangePayloadDto payload);
    }

}