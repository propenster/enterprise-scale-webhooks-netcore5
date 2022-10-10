using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAgentWeb.Data;
using TravelAgentWeb.Dtos;

namespace TravelAgentWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly TravelAgentDbContext _context;

        public NotificationController(TravelAgentDbContext context)
        {
            _context = context;
        }



        [HttpPost]
        public async Task<ActionResult> FlightChanged(FlightDetailUpdateDto flightDetailUpdateDto)
        {
            Console.WriteLine($"Webhook Received from: {flightDetailUpdateDto.Publisher} ");

            var secret = await _context.SubscriptionSecrets.FirstOrDefaultAsync(x => x.Secret == flightDetailUpdateDto.Secret && x.Publisher == flightDetailUpdateDto.Publisher);
            if (secret is null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid secret - Ignore Webhook");
                Console.ResetColor();

                return Ok();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Valid Webhook!");
                Console.WriteLine($"Old Price {flightDetailUpdateDto.OldPrice}, New Price {flightDetailUpdateDto.NewPrice}");
                Console.ResetColor();

                return Ok();

            }




        }




    }

}