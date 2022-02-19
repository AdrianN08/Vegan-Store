using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeganStore.Models.Entities
{
    public class OrderDetail
    {
        public OrderDetail()
        {

        }
        public OrderDetail(int id, int orderId, Order order, int productId, int quantity, decimal price)
        {
            Id = id;
            OrderId = orderId;
            Order = order;
            ProductId = productId;
            Quantity = quantity;
            Price = price;
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        [ValidateNever]
        public Order Order { get; set; }

        [Required]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product Product { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "money")]
        public decimal Price { get; set; }
    }
}
