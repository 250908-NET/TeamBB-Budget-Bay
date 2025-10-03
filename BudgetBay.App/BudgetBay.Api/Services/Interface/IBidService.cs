/*
    Name: IBidService.cs
    Purpose: Service layer interface for bid-related operations.
    Child Class: BidService.cs
*/

using BudgetBay.Models;
using BudgetBay.Repositories;


namespace BudgetBay.Services
{
    public interface IBidService
    {
        public Task<List<Bid>> GetAllBids(); // get all bids
        public Task<Bid> CreateBid(Bid newBid); // create new bid
        public Task CancelBid(int productId, int userId); // cancel a bi
         
        public Task<List<Bid>> AuctionsWonByUserId(int userId); // get all auctions won by a user
        public Task<decimal?> GetHighestBid(int ProductId); // get highest bid for a product

        public Task<List<Bid>> GetBidsByProductId(int ProductId); // get bid by product id
        public Task<List<Bid>> GetBidsByUserId(int UserId); // get bid by user id
        public Task<bool?> _CheckValidBid(Bid newBid, decimal price); // check if bid is valid
    }
}