namespace BudgetBay.Models
{
    public class Address
    {
        public int Id { get; set; }
        public int StreetNumber { get; set; }
        public string StreetName { get; set; }
        public string? AptNumber { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string zipCode { get; set; }
    }
}