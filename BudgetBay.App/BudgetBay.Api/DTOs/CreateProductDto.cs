using BudgetBay.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BudgetBay.Api.DTOs
{
    public class CreateProductDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string ImageUrl { get; set; } = string.Empty;

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ProductCondition? Condition { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        public int SellerId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Starting price must be non-negative")]
        public decimal StartingPrice { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Current price must be non-negative")]
        public decimal? CurrentPrice { get; set; }
    }
}
