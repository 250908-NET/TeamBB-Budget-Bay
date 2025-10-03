using AutoMapper;
using BudgetBay.DTOs;
using BudgetBay.Models;

namespace BudgetBay.Data
{
    public class MappingProfile: Profile{
        public MappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<Bid, BidDto>().ReverseMap();
            CreateMap<Address, AddressDto>();
            CreateMap<Product, CreateProductDto>().ReverseMap();
            CreateMap<Product, UpdateProductDto>().ReverseMap().ForAllMembers(opt => opt.Condition((src, dest, srcValue) => srcValue != null));
        }
    }
}