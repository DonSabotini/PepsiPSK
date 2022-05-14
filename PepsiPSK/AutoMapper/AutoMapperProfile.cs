using AutoMapper;
using PepsiPSK.Entities;
using PepsiPSK.Models.Flower;
using PepsiPSK.Models.Transaction;

namespace PepsiPSK.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AddFlowerDto, Flower>();
            CreateMap<AddTransactionDto, Transaction>();
        }
    }
}
