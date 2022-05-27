using AutoMapper;
using PepsiPSK.Entities;
using PepsiPSK.Models.Flower;
using PepsiPSK.Models.Photos;
using PepsiPSK.Models.User;
using PepsiPSK.Models.Order;

namespace PepsiPSK.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AddFlowerDto, Flower>();
            CreateMap<Flower, GetFlowerDto>();
            CreateMap<User, UserInfoDto>();
            CreateMap<Photo, PhotoListDto>();
            CreateMap<Order, GetOrderDto>()
                .ForMember(x => x.OrderedFlowerInfo, opt => opt.MapFrom(x => x.Items))
                .ForMember(x => x.OrderNumber, opt => opt.MapFrom(x => "ORD" +  x.OrderNumber.ToString("00000")));
            CreateMap<AddOrderDto, Order>();
            CreateMap<Flower, FlowerItem>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.OrderId, opt => opt.Ignore())
                .ForMember(x => x.FlowerId, opt => opt.MapFrom(x => x.Id));
            CreateMap<FlowerItem, OrderedFlowerInfoDto>();
        }
    }
}
