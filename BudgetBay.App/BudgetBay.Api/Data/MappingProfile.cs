using System.Net.Http.Headers;
using AutoMapper;
using BudgetBay.DTOs;
using BudgetBay.Models;

namespace BudgetBay.Data
{
    public class MappingProfile: Profile{
        public MappingProfile()
        {
            CreateMap<User, UserDto>();

            CreateMap<UpdateUserDto, User>().ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<AddressDto, Address>().ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Bid, BidDto>().ReverseMap();

            CreateMap<Bid, UserBidDto>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Bidder.Username))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount));

            CreateMap<Address, AddressDto>();
            CreateMap<Product, CreateProductDto>().ReverseMap();
            CreateMap<Product, UpdateProductDto>().ReverseMap().ForAllMembers(opt => opt.Condition((src, dest, srcValue) => srcValue != null));
            CreateMap<Product, ProductDetailDto>();
            CreateMap<Product, ProductDto>();
        }
    }
}