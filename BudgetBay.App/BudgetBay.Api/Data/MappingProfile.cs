using AutoMapper;
using BudgetBay.Models;
using BudgetBay.DTOs;

namespace BudgetBay.Data
{
    public class MappingProfile: Profile{
        public MappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<Bid, BidDto>();
            CreateMap<Address, AddressDto>();
        }
    }
}