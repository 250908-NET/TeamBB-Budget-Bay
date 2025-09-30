using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetBay.Models
{
    public class Bid 
    {
        [Key]
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int BidderId { get; set; }
        public Customer Bidder { get; set; }
    }
}