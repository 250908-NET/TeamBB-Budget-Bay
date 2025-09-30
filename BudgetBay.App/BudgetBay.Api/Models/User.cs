using System.ComponentModel.DataAnnotations;
namespace BudgetBay.Models {
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Username { get; set; } = "";
        [Required, MaxLength(255)]
        public string Email { get; set; } = "";
        [Required]
        public string PasswordHash { get; set; } = "";
        public Address Address { get; set; } = new();

        public User(string username, string email, string passwordHash)
        {
            Username = username;
            Email = email;
            PasswordHash = passwordHash;
        }
    }
}
