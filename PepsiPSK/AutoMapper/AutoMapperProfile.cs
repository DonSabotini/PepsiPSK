using AutoMapper;
using PepsiPSK.Entities;
using PepsiPSK.Models.Flower;

namespace PepsiPSK.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Flower, FlowerDto>();
        }
    }
}
