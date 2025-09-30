using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetBay.Models
{
    public class Bid 
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int BidderId { get; set; }
        public User Bidder { get; set; } = null!;
    }
}
