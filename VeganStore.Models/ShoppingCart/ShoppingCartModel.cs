using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeganStore.Models.Product;

namespace VeganStore.Models.ShoppingCart
{
    public class ShoppingCartModel
    {
        public ShoppingCartModel()
        {

        }

        public ShoppingCartModel(int productId, int quantity, string appUserId)
        {
            ProductId = productId;
            Quantity = quantity;
            AppUserId = appUserId;
        }

        public ShoppingCartModel(int id, int productId, int quantity, string appUserId)
        {
            Id = id;
            ProductId = productId;
            Quantity = quantity;
            AppUserId = appUserId;
        }

        public ShoppingCartModel(int id, int productId, int quantity, string appUserId, ProductCartViewModel product)
        {
            Id = id;
            ProductId = productId;
            Quantity = quantity;
            AppUserId = appUserId;
            Product = product;
        }

        public int Id { get; set; }
        public int ProductId { get; set; }   
        public int Quantity { get; set; }
        public string AppUserId { get; set; }
        public ProductCartViewModel Product { get; set; }
        public decimal Price { get; set; }
    }
}
