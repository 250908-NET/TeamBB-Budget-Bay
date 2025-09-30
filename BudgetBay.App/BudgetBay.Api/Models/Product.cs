using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace BudgetBay.Api.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public ProductCondition Condition { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal StartingPrice { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal CurrentPrice { get; set; }

        public int SellerId { get; set; }

        public int WinnerId { get; set; }

    }
}
