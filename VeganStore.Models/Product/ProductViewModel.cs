using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeganStore.Models.Product
{
    public class ProductViewModel
    {
        public ProductViewModel()
        {

        }
        public ProductViewModel(int id, string name, string articleNumber, decimal regularPrice, decimal salePrice, string subCategoryName, string categoryName)
        {
            Id = id;
            Name = name;
            ArticleNumber = articleNumber;
            RegularPrice = regularPrice;
            SalePrice = salePrice;
            SubCategoryName = subCategoryName;
            CategoryName = categoryName;
        }

        public int Id { get; set; }
        public string Name { get; set; }       
        [Display(Name = "Artikelnummer")]
        public string ArticleNumber { get; set; }     
        [Display(Name = "Ordinare Pris")]
        public decimal RegularPrice { get; set; }       
        [Display(Name = "Pris")]
        public decimal SalePrice { get; set; }
               
        [Display(Name = "Under kategori")]
        public string SubCategoryName { get; set; }
        
        [Display(Name = "Katergori")]
        public string CategoryName { get; set; }
    }
}
