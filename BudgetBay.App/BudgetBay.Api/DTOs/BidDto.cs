namespace BudgetBay.DTOs 
{

    public class BidDto
    {
        [Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }
        public int ProductId { get; set; }
        public int BidderId { get; set; }
    }
}
