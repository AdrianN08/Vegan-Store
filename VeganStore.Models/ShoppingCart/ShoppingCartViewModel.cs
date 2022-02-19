using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeganStore.Models.Entities;

namespace VeganStore.Models.ShoppingCart
{
    public class ShoppingCartViewModel
    {
        public IEnumerable<ShoppingCartModel> Carts { get; set; }
        public Order Order { get; set; }
    }
}
