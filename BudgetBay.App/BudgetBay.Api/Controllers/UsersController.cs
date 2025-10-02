using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using BudgetBay.Services;
using BudgetBay.Models;
using BudgetBay.DTOs;
using Serilog;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Runtime.InteropServices;
namespace BudgetBay.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //"/users"
    public class UsersController : ControllerBase
    {

        private readonly ILogger<UsersController> _logger;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UsersController(ILogger<UsersController> logger, IMapper mapper, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
            _mapper = mapper;
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

        [HttpPost("/{userId}/address", Name = "CreateUserAddress")]
        public async Task<IActionResult> CreateUserAddress(int userId)
        {
            throw new NotImplementedException();

        }

        [HttpPut("/{userId}/address", Name = "UpdateUserAddress")]
        public async Task<IActionResult> UpdateUserAddress(int userId)
        {
            throw new NotImplementedException();

        }

        [HttpPut("/{id}", Name = "UpdateUserById")]
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
