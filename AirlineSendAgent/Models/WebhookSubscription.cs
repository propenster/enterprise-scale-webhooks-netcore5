using System.ComponentModel.DataAnnotations;

namespace AirlineSendAgent.Models
{

    public class WebhookSubscription
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string WebhookURI { get; set; } = string.Empty;
        [Required]
        public string Secret { get; set; } = string.Empty;
        [Required]
        public string WebhookType { get; set; } = string.Empty;
        [Required]
        public string WebhookPublisher { get; set; } = string.Empty;






    }





}