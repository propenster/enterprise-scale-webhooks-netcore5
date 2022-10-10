using AutoMapper;
using AirlineWeb.Dtos;
using AirlineWeb.Models;

namespace AirlineWeb.Profiles
{
    public class WebhookSubscriptionProfile : Profile
    {

        public WebhookSubscriptionProfile()
        {
            CreateMap<WebhookSubscriptionCreateDto, WebhookSubscription>().ReverseMap();
            CreateMap<WebhookSubscription, WebhookSubscriptionReadDto>().ReverseMap();

            CreateMap<FlightDetail, FlightDetailReadDto>().ReverseMap();
            CreateMap<FlightDetailCreateDto, FlightDetail>().ReverseMap();





        }
    }

}