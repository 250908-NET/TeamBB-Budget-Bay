using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetBay.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public ProductCondition Condition { get; set; }
        public DateTime StartTime { get; set; } = DateTime.UtcNow;
        public DateTime EndTime { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal StartingPrice { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal? CurrentPrice { get; set; }
        [ForeignKey("Seller")]
        public int SellerId { get; set; }
        public User? Seller { get; set; }
        public int? WinnerId { get; set; }
        public User? Winner { get; set; }
        public ICollection<Bid> Bids { get; set; } = [];
    }
}
