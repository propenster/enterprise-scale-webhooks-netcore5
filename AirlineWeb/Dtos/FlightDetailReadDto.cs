using System.ComponentModel.DataAnnotations;

namespace AirlineWeb.Dtos
{

    public class FlightDetailReadDto
    {
        public int Id { get; set; }
        public string FlightCode { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }





}