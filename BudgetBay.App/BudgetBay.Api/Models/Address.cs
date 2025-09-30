using System.ComponentModel.DataAnnotations;
namespace BudgetBay.Models
{
    public class Address
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int StreetNumber { get; set; }
        [Required]
        public string StreetName { get; set; }
        public string? AptNumber { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string zipCode { get; set; }

        public Address(int streetNumber, string streetName, string city, string state, string zipCode, string? aptNumber = null)
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