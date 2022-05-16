using AutoMapper;
using PepsiPSK.Entities;
using PepsiPSK.Models.Flower;
using PepsiPSK.Models.Order;

namespace PepsiPSK.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AddFlowerDto, Flower>();
            CreateMap<Flower, GetFlowerDto>();
            CreateMap<Order, GetOrderDto>();
        }
    }
}
