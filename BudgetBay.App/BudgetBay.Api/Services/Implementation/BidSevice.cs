/*
    Name: BidService.cs
    Purpose: Service layer for bid-related operations, this will interact with the bid repository and all the functions that are needed for the controller
    Parent Class: IBidService.cs
*/
using BudgetBay.Models;
using BudgetBay.Repositories;


namespace BudgetBay.Services
{
    public class BidService : IBidService
    {
        private readonly ILogger<BidService> _logger;

        private readonly IBidRepository _bidRepo; // declare bid repository

        private readonly IProductRepository _productRepo; // declare product repository

        private readonly IUserRepository _userRepo; // declare user repository

        public BidService(ILogger<BidService> logger, IBidRepository bidRepository, IProductRepository productRepository, IUserRepository userRepository)
        {
            _logger = logger;
            _bidRepo = bidRepository;
            _productRepo = productRepository;
            _userRepo = userRepository;
        }

        public async Task<Bid> CreateBid(Bid newBid)
        {
            return await _bidRepo.AddAsync(newBid); // call AddAsync method to add newBid to database
        } // create new bid
        public async Task CancelBid(int bidId)
        {
            var bid = await _bidRepo.GetByIdAsync(bidId); // get bid by id
            if (bid == null) // check if the Id is valid, console will get a warning if not
            {
                _logger.LogWarning($"No bid found with ID: {bidId}");
                return;
            }
            else
            {
                await _bidRepo.DeleteAsync(bid);
                await _bidRepo.SaveChangesAsync();
            }
        } // cancel a bid
        public async Task<Bid> GetHighestBid(int ProductId)
        {
            var productBids = await _productRepo.GetByIdAsync(ProductId); // get product by id
            if (productBids == null || !productBids.Bids.Any()) // check if Id is Valid or if there are any bids
            {
                _logger.LogWarning($"{(productBids == null ? "No Product found for the product ID:" : "No Bids Were found for the product ID:")} {ProductId}");
                return null;
            }
            var highestBid = await _productRepo.GetHighestBidAsync(ProductId);
            return highestBid;
        } // get highest bid for a product

        public async Task<List<Bid>> GetBidsByProductId(int ProductId)
        {
            var productBids = await _productRepo.GetByIdAsync(ProductId); // get product by id
            if (productBids == null || !productBids.Bids.Any()) // check if Id is Valid or if there are any bids
            {
                _logger.LogWarning($"{(productBids == null ? "No Product found for the product ID:" : "No Bids Were found for the product ID:")} {ProductId}");
                return null;
            }
            return productBids.Bids.ToList(); // return all bids for the product
        } // get bid by product id
        public async Task<List<Bid>> GetBidsByUserId(int UserId)
        {
            var userBids = await _userRepo.GetByIdAsync(UserId); // get user by id
            if (userBids == null || !userBids.Bids.Any()) // check if Id is Valid or if there are any bids
            {
                _logger.LogWarning($"{(userBids == null ? "No User found for the user ID:" : "No Bids Were found for the user ID:")} {UserId}");
                return null;
            }
            return userBids.Bids.ToList(); // return all bids for the user
        } // get bid by user id
        public async Task<bool?> _CheckValidBid(int ProductId, decimal price)
        {
            var highestBid = await GetHighestBid(ProductId); // get highest bid for the product
            _logger.LogInformation($"Checking if there are bids for the product ID: {ProductId}");
            if (highestBid != null) // check if there are any bids
            {
                // there are bids, check if the new bid is higher than the highest bid
                _logger.LogInformation($"Highest bid for product ID {ProductId} is {highestBid.Amount}");
                return price > highestBid.Amount;
            }
            else
            {
                // no bids, check if the new bid is higher than the starting price
                var product = await _productRepo.GetByIdAsync(ProductId); // get product by id
                if (product == null)
                {
                    _logger.LogWarning($"No Product found for the product ID: {ProductId}");
                    return null;
                }
                _logger.LogInformation($"No bids found for product ID {ProductId}. Checking against starting price {product.StartingPrice}");
                return price > product.StartingPrice;
            }
        } // check if bid is valid

     }
}