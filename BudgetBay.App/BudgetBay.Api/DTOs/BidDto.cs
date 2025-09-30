namespace BudgetBay.DTOs 
{

    public class BidDto
    {
        public decimal Amount { get; set; }
        public int ProductId { get; set; }
        public int BidderId { get; set; }
    }
}