using System.ComponentModel.DataAnnotations;
namespace BudgetBay.DTOs
{
    public class LoginUserDto
    {
        public string? Email { get; set; } = null!;
        public string? Password { get; set; } = null!;
    }
}