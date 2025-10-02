using AutoMapper;
using BudgetBay.Models;
using BudgetBay.DTOs;

namespace BudgetBay.Data
{
    public class MappingProfile: Profile{
        public MappingProfile()
        {
            CreateMap<User, UserDto>();

            CreateMap<UpdateUserDto, User>().ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<AddressDto, Address>().ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Bid, BidDto>();
            
            CreateMap<Address, AddressDto>();
        }
    }
}