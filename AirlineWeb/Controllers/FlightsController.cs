using AirlineWeb.Data;
using AirlineWeb.Dtos;
using AirlineWeb.MessageBus;
using AirlineWeb.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AirlineWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class FlightsController : ControllerBase
    {
        private readonly AirlineDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMessageBusClient _messageBus;

        public FlightsController(AirlineDbContext context, IMapper mapper, IMessageBusClient messageBus)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _messageBus = messageBus;
        }


        [HttpGet("{flightCode}/GetFlightsByCode", Name = "GetFlightDetailsByCode")]
        public async Task<ActionResult<FlightDetailReadDto>> GetFlightDetailsByCode([FromRoute] string flightCode)
        {
            var flight = await _context.FlightDetails.FirstOrDefaultAsync(x => x.FlightCode == flightCode);
            if (flight is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<FlightDetailReadDto>(flight));


        }

        [HttpPost]
        [Route("createFlight")]
        public async Task<ActionResult<FlightDetailReadDto>> CreateFlight([FromBody] FlightDetailCreateDto request)
        {
            var flight = await _context.FlightDetails.FirstOrDefaultAsync(x => x.FlightCode == request.FlightCode);
            if (!(flight is null))
            {
                return NoContent();
            }


            var flightDetailModel = _mapper.Map<FlightDetail>(request);

            try
            {
                await _context.FlightDetails.AddAsync(flightDetailModel);
                await _context.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {

                return BadRequest(ex.Message);
            }

            var flightDetailReadDto = _mapper.Map<FlightDetailReadDto>(flightDetailModel);
            return CreatedAtRoute(nameof(GetFlightDetailsByCode), new { flightCode = flightDetailReadDto.FlightCode }, flightDetailReadDto);


        }
        // [HttpGet]
        // [Route("{secret}", Name = "GetSubscriptionBySecret")]
        // public async Task<ActionResult<WebhookSubscriptionReadDto>> GetSubscriptionBySecret(string secret)
        // {
        //     var subscription = await _context.WebhookSubscriptions.FirstOrDefaultAsync(x => x.Secret == secret);
        //     if (subscription is null)
        //     {
        //         return NotFound();
        //     }
        //     return Ok(_mapper.Map<WebhookSubscription>(subscription));

        // }

        [HttpGet("{id}")]
        public async Task<ActionResult<FlightDetailReadDto>> GetFlightById([FromRoute] int id)
        {
            var flight = await _context.FlightDetails.FirstOrDefaultAsync(x => x.Id == id);
            if (flight is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<FlightDetailReadDto>(flight));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateFlightDetail(int id, FlightUpdateDto request)
        {
            var flight = await _context.FlightDetails.FirstOrDefaultAsync(x => x.Id == id);
            if (flight is null)
            {
                return NotFound();
            }

            decimal oldPrice = flight.Price;


            _mapper.Map(request, flight);
            try
            {
                await _context.SaveChangesAsync();
                if (oldPrice != flight.Price)
                {
                    Console.WriteLine("Price Changed - Place message on Message Bus");
                    var message = new NotificationMessageDto
                    {
                        OldPrice = oldPrice,
                        NewPrice = flight.Price,
                        WebhookType = "pricechange",
                        FlightCode = flight.FlightCode
                    };
                    _messageBus.SendMessage(message); //publish webhook to Exchange queue for consumers


                }
                else
                {
                    Console.WriteLine("No Price Change.");
                }


                return NoContent();

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }






        }

    }






}