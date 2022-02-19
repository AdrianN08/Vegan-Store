using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeganStore.Models.Product;

namespace VeganStore.Models.Entities
{
    public class ShoppingCart
    {
        public ShoppingCart()
        {

        }
        public ShoppingCart(int id, int productId, int quantity, string appUserId)
        {
            Id = id;
            ProductId = productId;
            Quantity = quantity;
            AppUserId = appUserId;
        }

        public ShoppingCart(int productId, int quantity, string appUserId)
        {
            ProductId = productId;
            Quantity = quantity;
            AppUserId = appUserId;
        }

        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product Product { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public string AppUserId { get; set; }       
    }
}
