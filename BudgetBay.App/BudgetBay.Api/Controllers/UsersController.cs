using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using BudgetBay.Services;
using BudgetBay.Models;
using BudgetBay.DTOs;
using Serilog;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Runtime.InteropServices;
using BudgetBay.Repositories;
namespace BudgetBay.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //"/users"
    public class UsersController : ControllerBase
    {

        private readonly ILogger<UsersController> _logger;
        private readonly IUserService _userService;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public UsersController(ILogger<UsersController> logger, IMapper mapper, IUserService userService, IProductService productService)
        {
            _logger = logger;
            _mapper = mapper;
            _userService = userService;
            _productService = productService;
        }

        [HttpGet("{id}", Name = "GetUserById")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            _logger.LogInformation("Getting user {id}", id);
            var user = await _userService.GetUserInfo(id);
            return user is not null ? Ok(_mapper.Map<UserDto>(user)) : NotFound();
        }

        [HttpGet("{id}/bids", Name = "GetAllUserBids")]
        public async Task<IActionResult> GetAllUserBids(int id)
        {
            throw new NotImplementedException();
        }

        //sellers products
        [HttpGet("{id}/products", Name = "GetAllSellerProducts")]
        public async Task<IActionResult> GetAllSellersProducts(int id)
        {
            throw new NotImplementedException();

        }

        [HttpGet("{id}/won-auctions", Name = "GetAllAuctionsWonByUser")]
        public async Task<IActionResult> GetAllAuctionsWonByUser(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost("{userId}/address", Name = "CreateUserAddress")]
        public async Task<IActionResult> CreateUserAddress(int userId, [FromBody] AddressDto dto)
        {
            var user = await _userService.GetUserInfo(userId);
            if (user is null)
            {
                return NotFound();
            }

            if (user.AddressId.HasValue)
            {
                return Conflict("User already has an address. Use PUT to update it.");
            }

            var newAddress = _mapper.Map<Address>(dto);
            newAddress = await _userService.CreateAddress(newAddress);

            user.AddressId = newAddress.Id;
            await _userService.UpdateUser(user);

            return Ok(_mapper.Map<AddressDto>(newAddress));
            // return CreatedAtRoute("GetUserAddress", new { userId }, _mapper.Map<AddressDto>(newAddress));
        }

        [HttpPut("{userId}/address", Name = "UpdateUserAddress")]
        public async Task<IActionResult> UpdateUserAddress(int userId, [FromBody] AddressDto dto)
        {
            var user = await _userService.GetUserInfo(userId);
            if (user is null || !user.AddressId.HasValue)
            {
                return NotFound();
            }

            var existingAddress = await _userService.GetUserAddressAsync(user.AddressId.Value);
            _mapper.Map(dto, existingAddress);

            var updatedAddress = await _userService.UpdateAddress(existingAddress!);
            return Ok(_mapper.Map<AddressDto>(updatedAddress));
        }


        [HttpPut("{id}", Name = "UpdateUserById")]
        public async Task<IActionResult> UpdateUserById(int id, [FromBody] UpdateUserDto dto)
        {
            _logger.LogInformation($"Updating user {id}");
            if (!await _userService.Exists(id))
            {
                return NotFound();
            }
            var existingUser = await _userService.GetUserInfo(id);
            if (existingUser is null)
            {
                return NotFound();
            }
            var updatedUser = _mapper.Map(dto, existingUser);

            var result = await _userService.UpdateUser(updatedUser);

            return result is not null ? Ok(_mapper.Map<UserDto>(result)) : NotFound();
            
        }
    }
}
