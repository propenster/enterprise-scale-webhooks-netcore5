using AirlineWeb.Data;
using AirlineWeb.Dtos;
using AirlineWeb.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AirlineWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WebhookSubscriptionController : ControllerBase
    {
        private readonly AirlineDbContext _context;
        private readonly IMapper _mapper;

        public WebhookSubscriptionController(AirlineDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost]
        [Route("CreateSubscription")]
        public async Task<ActionResult<WebhookSubscriptionReadDto>> CreateSubscription([FromBody] WebhookSubscriptionCreateDto request)
        {
            var subscription = _context.WebhookSubscriptions
            .FirstOrDefault(
                x => x.WebhookURI == request.WebhookURI
            );
            if (!(subscription is null))
            {
                return NoContent();
            }
            subscription = _mapper.Map<WebhookSubscription>(request);
            subscription.Secret = Guid.NewGuid().ToString();
            subscription.WebhookPublisher = "PanAus";

            try
            {
                await _context.WebhookSubscriptions.AddAsync(subscription);
                await _context.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }

            var response = _mapper.Map<WebhookSubscriptionReadDto>(subscription);

            return CreatedAtRoute(nameof(GetSubscriptionBySecret), new { secret = response.Secret }, response);

        }

        [HttpGet]
        [Route("{secret}", Name = "GetSubscriptionBySecret")]
        public async Task<ActionResult<WebhookSubscriptionReadDto>> GetSubscriptionBySecret(string secret)
        {
            var subscription = await _context.WebhookSubscriptions.FirstOrDefaultAsync(x => x.Secret == secret);
            if (subscription is null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<WebhookSubscription>(subscription));

        }


    }




}