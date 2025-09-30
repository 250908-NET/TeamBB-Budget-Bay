using System.ComponentModel.DataAnnotations;
namespace BudgetBay.DTOs
{
    public class RegisterUserDto
    {
        [MaxLength(50)]
        public string? Username { get; set; } = null!;
        [EmailAddress, MaxLength(255)]
        public string? Email { get; set; } = null!;
        public string? Password { get; set; } = null!;
    }
}