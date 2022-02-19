using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeganStore.Models.Entities
{
    public class Product
    {
        public Product()
        {

        }

        public Product(string name, string articleNumber, decimal regularPrice, decimal salePrice, int subCategoryId)
        {
            Name = name;
            ArticleNumber = articleNumber;
            RegularPrice = regularPrice;
            SalePrice = salePrice;
            SubCategoryId = subCategoryId;
        }

        public Product(int id, string name, string articleNumber, decimal regularPrice, decimal salePrice, int subCategoryId)
        {
            Id = id;
            Name = name;
            ArticleNumber = articleNumber;
            RegularPrice = regularPrice;
            SalePrice = salePrice;
            SubCategoryId = subCategoryId;
        }

        [Key]
        public int Id { get; set; }

        [Required]       
        public string Name { get; set; }
        [Required]
        public string ArticleNumber { get; set; }

        [Required]
        [Range(1, 10000)]
        [Column(TypeName = "money")]
        public decimal RegularPrice { get; set; }
        [Required]
        [Range(1, 10000)]
        [Column(TypeName = "money")]
        public decimal SalePrice { get; set; }
        [Required]
        public int SubCategoryId { get; set; }
        public virtual SubCategory SubCategory { get; set; }
            
    }
}
