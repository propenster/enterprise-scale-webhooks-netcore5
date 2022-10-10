using System.ComponentModel.DataAnnotations;

namespace AirlineWeb.Dtos
{

    public class FlightUpdateDto
    {
        [Required]
        public string FlightCode { get; set; } = string.Empty;
        [Required]
        public decimal Price { get; set; }
    }





}