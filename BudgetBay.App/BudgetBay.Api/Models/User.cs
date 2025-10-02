using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetBay.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Username { get; set; } = null!;

        [Required, MaxLength(255)]
        public string Email { get; set; } = null!;

        [Required]
        public string PasswordHash { get; set; } = null!;

        public int? AddressId { get; set; }
        public Address? Address { get; set; } = null!;


        [InverseProperty("Seller")]
        public ICollection<Product> ProductsForSale { get; set; } = new List<Product>();

        [InverseProperty("Winner")]
        public ICollection<Product> WonProducts { get; set; } = new List<Product>();

        [InverseProperty("Bidder")] 
        public ICollection<Bid> Bids { get; set; } = new List<Bid>();

        public User() { }

        public User(string username, string email, string passwordHash)
        {
            Username = username;
            Email = email;
            PasswordHash = passwordHash;
        }
    }
}