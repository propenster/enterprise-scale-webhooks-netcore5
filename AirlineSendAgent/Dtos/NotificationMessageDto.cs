namespace AirlineSendAgent.Dtos
{

    public class NotificationMessageDto
    {

        public string Id { get; }
        public string WebhookType { get; set; } = string.Empty;
        public string FlightCode { get; set; } = string.Empty;
        public decimal OldPrice { get; set; }
        public decimal NewPrice { get; set; }
    }





}