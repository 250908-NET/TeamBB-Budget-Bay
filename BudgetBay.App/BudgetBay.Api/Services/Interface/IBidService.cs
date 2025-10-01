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
        public Task<Bid> CreateBid(Bid newBid); // create new bid
        public Task CancelBid(int bidId); // cancel a bid
        public Task<Bid> GetHighestBid(int ProductId); // get highest bid for a product

        public Task<List<Bid>> GetBidsByProductId(int ProductId); // get bid by product id
        public Task<List<Bid>> GetBidsByUserId(int UserId); // get bid by user id
        public Task<bool?> _CheckValidBid(int ProductId, decimal price); // check if bid is valid
    }
}