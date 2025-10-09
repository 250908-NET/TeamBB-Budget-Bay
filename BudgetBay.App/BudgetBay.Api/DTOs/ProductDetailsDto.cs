using BudgetBay.Models;
using System.Collections.Generic;
using System;

namespace BudgetBay.DTOs
{
    public class ProductDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public ProductCondition Condition { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal StartingPrice { get; set; }
        public decimal? CurrentPrice { get; set; }
        public UserDto? Seller { get; set; }
        public ICollection<UserBidDto> Bids { get; set; } = [];
    }
}