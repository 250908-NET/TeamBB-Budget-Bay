using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetBay.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public ProductCondition Condition { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal StartingPrice { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal CurrentPrice { get; set; }

        public int SellerId { get; set; }
        public User Seller { get; set; } = null!;

        public int? WinnerId { get; set; }
        public User? Winner { get; set; }

        public ICollection<Bid> Bids { get; set; } = new List<Bid>();
    }
}