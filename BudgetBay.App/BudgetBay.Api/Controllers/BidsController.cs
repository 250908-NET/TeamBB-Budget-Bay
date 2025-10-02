/*
File Name: BidController.cs
Description: This Controller handles all bid-related HTTP requests, including creating bids, canceling bids, and retrieving bids by product or user ID.
Requests Handled:
GET  /bids  #Get all bids from bids table
DELETE  /{productId}/user/{userId}/bids   #Delete all bids user made on product
GET   /{productId}/bids  #Get all bids made on a product
GET   /{productid}/bids/highest  #Get the highest bid on a product
POST  /{productid}/bids  #Create bid for product

Parent Class: ControllerBase
*/

using BudgetBay.Services;
using BudgetBay.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BudgetBay.DTOs;
using BudgetBay.Models;
using BudgetBay.Services;
using AutoMapper;
using Serilog;


namespace BudgetBay.Controllers
{
    [ApiController]
    [Route("api/[Products]")]
    // /api/products
    public class BidsController : ControllerBase
    {
        private readonly ILogger<BidsController> _logger;
        private readonly IBidService _bidService;
        private readonly IMapper _mapper;
        // Constructor 
        public BidsController(ILogger<BidsController> logger, IBidService bidService, IMapper mapper)
        {
            _logger = logger;
            _bidService = bidService;
            _mapper = mapper;
        }

        // Metods

        // Get all bids from bids table
        [HttpGet(Name = "GetAllBids")]
        public async Task<IActionResult> GetAllBids()
        {
            var bids = await _bidService.GetAllBids();


            var bidsDTOs = _mapper.Map<List<BidDto>>(bids);
            return Ok(bidsDTOs);
        }

        // Delete all bids user made on product
        [HttpDelete("{productId}/user/{userId}/bids", Name = "DeleteBidsByProductAndUser")]
        public async Task<IActionResult> DeleteBidsByProductAndUser(int productId, int userId)
        {
            await _bidService.CancelBid(productId, userId);

            return Ok();
        }

        // Get all bids made on a product
        [HttpGet("{productId}/bids", Name = "GetBidsByProductId")]
        public async Task<IActionResult> GetBidsByProductId(int productId)
        {
            var bids = await _bidService.GetBidsByProductId(productId);



            return bids is not null ? Ok(_mapper.Map<List<BidDto>>(bids)) : NotFound($"No bids found for product ID {productId}");
        }

        // Get the highest bid on a product
        [HttpGet("{productId}/bids/highest", Name = "GetHighestBidByProductId")]
        public async Task<IActionResult> GetHighestBidByProductId(int productId)
        {
            var highestBid = await _bidService.GetHighestBid(productId);

            return highestBid is not null ? Ok(highestBid) : NotFound($"No bids found for product ID {productId}");
        }

        // Create bid for product
        [HttpPost("{productId}/bids", Name = "CreateBidForProduct")]
        public async Task<IActionResult> CreateBidForProduct(int productId, [FromBody] Bid newBid)
        {
            newBid.ProductId = productId;

            var createBid = await _bidService.CreateBid(newBid);

            return createBid is not null ? Created("/{createBid.Id}/bids", _mapper.Map<BidDto>(createBid)) : BadRequest("Bid could not be created. Please check the provided data and try again.");
        }

    }

    
}