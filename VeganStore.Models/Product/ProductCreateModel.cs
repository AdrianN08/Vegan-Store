using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeganStore.Models.Product
{
    public class ProductCreateModel
    {
        public ProductCreateModel()
        {

        }
        public ProductCreateModel(string name, string articleNumber, decimal regularPrice, decimal salePrice, string subCategory)
        {
            Name = name;
            ArticleNumber = articleNumber;
            RegularPrice = regularPrice;
            SalePrice = salePrice;
            SubCategoryName = subCategory;
        }
        [Required]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Artikelnummer")]
        public string ArticleNumber { get; set; }
        [Required]
        [Display(Name = "Ordinare Pris")]
        public decimal RegularPrice { get; set; }
        [Required]
        [Display(Name = "Pris")]
        public decimal SalePrice { get; set; }
        [Required]
        [Display(Name = "Under Kategori")]
        public string SubCategoryName { get; set; }
    }
}
