namespace TravelAgentWeb.Dtos
{
    public class FlightDetailChangePayloadDto
    {
        public string WebhookURI { get; set; } //so we know which particular client we are sending to!
        public string Publisher { get; set; }
        public string Secret { get; set; }
        public decimal OldPrice { get; set; }
        public decimal NewPrice { get; set; }
        public string WebhookType { get; set; }
        public string FlightCode {get; set;}
    }

}