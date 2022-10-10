using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace AirlineWeb.Dtos
{

    public class FlightDetailCreateDto
    {
        [Required]
        [JsonProperty("flightCode")]
        public string FlightCode { get; set; } = string.Empty;
        [Required]
        [JsonProperty("price")]
        public decimal Price { get; set; }
    }





}