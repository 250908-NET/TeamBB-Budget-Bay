using BudgetBay.Models;

namespace BudgetBay.DTOs
{
    public class AddressDto
    {
        public int Id { get; set; }
        public int StreetNumber { get; set; }
        public string StreetName { get; set; }
        public string? AptNumber { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }
}