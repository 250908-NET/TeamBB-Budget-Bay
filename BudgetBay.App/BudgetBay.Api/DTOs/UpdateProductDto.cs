using BudgetBay.Models;
using System.ComponentModel.DataAnnotations;

namespace BudgetBay.DTOs
{
    public class UpdateProductDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        [Url]
        public string? ImageUrl { get; set; }
        public ProductCondition Condition { get; set; }
        public DateTime? EndTime { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Starting price must be non-negative")]
        public decimal? StartingPrice { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Current price must be non-negative")]
        public decimal? CurrentPrice { get; set; }
    }
}
