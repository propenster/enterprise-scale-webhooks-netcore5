using AutoMapper;
using AirlineWeb.Dtos;
using AirlineWeb.Models;

namespace AirlineWeb.Profiles
{
    public class FlightDetailProfile : Profile
    {

        public FlightDetailProfile()
        {
            CreateMap<FlightDetail, FlightDetailReadDto>().ReverseMap();
            CreateMap<FlightDetailCreateDto, FlightDetail>().ReverseMap();
            CreateMap<FlightUpdateDto, FlightDetail>().ReverseMap();

        }
    }

}