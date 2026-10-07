using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceApp.Models
{
    public class CartItems
    {

        // Database Variable Initialisation

        public int Id {get;set;}
        public int productId{get;set;}
        public string productName{get;set;} = string.Empty;
        [Column(TypeName="decimal(18,2)")]
        public decimal Price {get;set;}
        public int Quantity{get;set;}


    }
}