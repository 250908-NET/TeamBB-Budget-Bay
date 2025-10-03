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
        public async Task<List<Bid>> GetAllBids()
        {
            _logger.LogInformation("Getting all bids from the database.");
            return await _bidRepo.GetAllAsync(); // call bid repo to get all bids
        } // get all bids

        public async Task<Bid> CreateBid(Bid newBid)
        {
            var isValidBid = await _CheckValidBid(newBid, newBid.Amount); // check if the bid is valid
            if (isValidBid == false) // check if the product id is valid
            {
                _logger.LogWarning($"Bid is not valid sending it back null");
                return null;
            }
            else
            {
                
                _logger.LogInformation($"Bid is valid, adding to database");
                await _productRepo.UpdateProductAsync(newBid.ProductId, (double)newBid.Amount); // update the product with the new highest bid
                return await _bidRepo.AddAsync(newBid); // call AddAsync method to add newBid to database
            }
        } // create new bid
        public async Task CancelBid(int productId, int userId)
        {
            var bidProducts = await GetBidsByProductId(productId); // get bid by id
            var bidUsers = await GetBidsByUserId(userId); // get bid by user id

            List<Bid> bid = bidProducts.Intersect(bidUsers).ToList(); // get the bid that matches both product id and user id
            if (bid == null || !bid.Any()) // check if the Id is valid, console will get a warning if not
            {
                _logger.LogWarning($"no bids found for product ID {productId} and user ID {userId}");
                return;
            }
            else
            {
                // update the product with the new highest bid
                foreach (var b in bid)
                {
                    await _bidRepo.DeleteAsync(b);
                    if (b.Amount >= (await GetHighestBid(b.ProductId)))
                    {
                        await _productRepo.UpdateProductAsync(b.ProductId, (double)(await GetHighestBid(b.ProductId) ?? 0)); // update the product with the new highest bid or 0 if there are no bids
                    }
                }
                
            }
        } // cancel a bid
        public async Task<List<Bid>> AuctionsWonByUserId(int userId)
        {
            var userBids = await GetBidsByUserId(userId); // get user by id
            List<Bid> wonBids = new List<Bid>();

            if (userBids == null || !userBids.Any()) // check if Id is Valid or if there are any bids
            {
                _logger.LogWarning($"No Bids Were found for the user ID: {userId}");
                return null;
            }
            else
            {
                foreach (var bid in userBids)
                {
                    var product = await _productRepo.GetByIdAsync(bid.ProductId); // get product by id
                    var highestBid = await GetHighestBid(bid.ProductId); // get highest bid for the product
                    if (product != null && highestBid != null && highestBid.Value == bid.Amount) // check if the user is the winner of the auction
                    {
                        wonBids.Add(bid); // add the bid to the list of won bids
                    }
                }
                return wonBids;
            }
            
        } // get all auctions won by a user
        public async Task<decimal?> GetHighestBid(int ProductId)
        {
            var productBids = await _bidRepo.GetByProductIdAsync(ProductId); // get product by id
            if (productBids == null || !productBids.Any()) // check if Id is Valid or if there are any bids
            {
                _logger.LogWarning($"{(productBids == null ? "No Product found for the product ID:" : "No Bids Were found for the product ID:")} {ProductId}");
                return null;
            }
            return productBids.Max(b => b.Amount);
        } // get highest bid for a product

        public async Task<List<Bid>> GetBidsByProductId(int ProductId)
        {
            var productBids = await _bidRepo.GetByProductIdAsync(ProductId); // get product by id
            if (productBids == null || !productBids.Any()) // check if Id is Valid or if there are any bids
            {
                _logger.LogWarning($"{(productBids == null ? "No Product found for the product ID:" : "No Bids Were found for the product ID:")} {ProductId}");
                return null;
            }
            return productBids.ToList(); // return all bids for the product
        } // get bid by product id
        public async Task<List<Bid>> GetBidsByUserId(int UserId)
        {
            var userBids = await _bidRepo.GetByUserIdAsync(UserId); // get user by id
            if (userBids == null || !userBids.Any()) // check if Id is Valid or if there are any bids
            {
                _logger.LogWarning($"{(userBids == null ? "No User found for the user ID:" : "No Bids Were found for the user ID:")} {UserId}");
                return null;
            }
            return userBids.ToList(); // return all bids for the user
        } // get bid by user id
        public async Task<bool?> _CheckValidBid(Bid newBid, decimal price)
        {
            var productBids = await _bidRepo.GetByProductIdAsync(newBid.ProductId); // get product by id

            _logger.LogInformation($"Checking if there are bids for the product ID: {newBid.ProductId}");
            if (productBids != null && productBids.Any()) // check if there are any bids
            {
                // there are bids, check if the new bid is higher than the highest bid
                var highestBid = productBids.Max(b => b.Amount);
                _logger.LogInformation($"Highest bid for product ID {newBid.ProductId} is {highestBid}");
                return price > highestBid;
            }
            else
            {
                
                // no bids, check if the new bid is higher than the starting price
                var product = await _productRepo.GetByIdAsync(newBid.ProductId); // get product by id
                if (product == null)
                {
                    _logger.LogWarning($"No Product found for the product ID: {newBid.ProductId}");
                    return false;
                }
                if (product.SellerId == newBid.BidderId)
                {
                    _logger.LogWarning($"Bidder ID {newBid.BidderId} is the same as Seller ID {product.SellerId} for product ID {newBid.ProductId}. Bid is invalid.");
                    return false; // bidder cannot be the seller
                }
                _logger.LogInformation($"No bids found for product ID {newBid.ProductId}. Checking against starting price {product.StartingPrice}");
                return price > product.StartingPrice; // check if the new bid is higher than the starting price
            }
        } // check if bid is valid

     }
}