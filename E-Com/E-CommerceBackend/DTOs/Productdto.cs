using System.ComponentModel.DataAnnotations;

namespace E_CommerceBackend.DTOs
{
    public class Productdto
    {
        public int UsertableId { get; set; }
        public string productName { get; set; }
        public string productCode { get; set; }
        public IFormFile? productImagePath { get; set; } // Path to the uploaded image
        public string category { get; set; }
        public string brand { get; set; }
        public float sellingPrice { get; set; }
        public float purchasePrice { get; set; }
        public string purchaseDate { get; set; }
        public int stock { get; set; }
    }
}
