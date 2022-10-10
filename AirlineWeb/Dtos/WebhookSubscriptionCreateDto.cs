using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace AirlineWeb.Dtos
{

    public class WebhookSubscriptionCreateDto
    {
        [Required]
        // [JsonProperty("webookUri")]
        public string WebhookURI { get; set; }
        // [Required]
        // public string Secret { get; set; } = string.Empty;
        [Required]
        // [JsonProperty("webookType")]
        public string WebhookType { get; set; }

    }


}