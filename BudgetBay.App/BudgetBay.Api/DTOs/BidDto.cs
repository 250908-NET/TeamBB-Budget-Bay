using BudgetBay.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BudgetBay.DTOs
{

    public class BidDto
    {
        public decimal Amount { get; set; }
        public int ProductId { get; set; }
        public int BidderId { get; set; }
    }
}
