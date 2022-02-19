using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeganStore.Models.Product
{
    public class ProductCartViewModel
    {
        public ProductCartViewModel()
        {

        }
        public ProductCartViewModel(string name, string articleNumber, decimal salePrice)
        {
            Name = name;
            ArticleNumber = articleNumber;
            SalePrice = salePrice;
        }

        public string Name { get; set; }
        [Display(Name = "Artikelnummer")]
        public string ArticleNumber { get; set; }
        [Display(Name = "Pris")]
        public decimal SalePrice { get; set; }
    
    }
}
