using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeganStore.Models.Product
{
    public class ProductModel
    {
        public ProductModel()
        {

        }

        public ProductModel(int id, string name, string articleNumber, decimal regularPrice, decimal salePrice, string subCategoryName)
        {
            Id = id;
            Name = name;
            ArticleNumber = articleNumber;
            RegularPrice = regularPrice;
            SalePrice = salePrice;
            SubCategoryName = subCategoryName;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        [Display(Name = "Artikelnummer")]
        public string ArticleNumber { get; set; }
        [Display(Name = "Ordinare Pris")]
        public decimal RegularPrice { get; set; }
        [Display(Name = "Pris")]
        public decimal SalePrice { get; set; }
        [Display(Name = "Under Kategori")]
        public string SubCategoryName { get; set; }
    }
}
