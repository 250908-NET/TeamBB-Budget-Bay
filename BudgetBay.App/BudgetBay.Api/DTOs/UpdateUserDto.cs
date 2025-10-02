using System.ComponentModel.DataAnnotations;
namespace BudgetBay.DTOs
{
    public class UpdateUserDto
    {
        [MaxLength(50)]
        public string? Username { get; set; } = null!;
        [EmailAddress, MaxLength(255)]
        public string? Email { get; set; } = null!;
    }
}