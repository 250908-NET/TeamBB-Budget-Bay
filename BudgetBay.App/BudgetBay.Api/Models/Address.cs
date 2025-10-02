using System.ComponentModel.DataAnnotations;

namespace BudgetBay.Models
{
    public class Address
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string StreetNumber { get; set; }  = null!;
        [Required]
        public string StreetName { get; set; } = null!;

        public string? AptNumber { get; set; } = null!;

        [Required]
        public string City { get; set; } = null!;

        [Required]
        public string State { get; set; } = null!;

        [Required]
        public string ZipCode { get; set; } = null!;

        public Address() { }

        public Address(string streetNumber, string streetName, string city, string state, string zipCode, string? aptNumber = null)
        {
            StreetNumber = streetNumber;
            StreetName = streetName;
            City = city;
            State = state;
            ZipCode = zipCode;
            AptNumber = aptNumber;
        }
    }
}