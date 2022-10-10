using System.ComponentModel.DataAnnotations;

namespace AirlineWeb.Dtos
{

    public class WebhookSubscriptionReadDto
    {
        public int Id { get; set; }
        public string WebhookURI { get; set; } = string.Empty;
        public string Secret { get; set; } = string.Empty;
        public string WebhookType { get; set; } = string.Empty;
        public string WebhookPublisher { get; set; } = string.Empty;






    }





}