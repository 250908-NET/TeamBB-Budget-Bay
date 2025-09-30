using System.ComponentModel.DataAnnotations.Schema;


namespace BudgetBay.Models
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
        [ForeignKey("sellerId")]
        public int? SellerId { get; set; }
        public User seller { get; set; }

        [ForeignKey("WinnerId")]
        public int?  WinnerId { get; set; }
        public User Winner { get; set; }

    }
}
