using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeganStore.Models.SubCategory
{
    public class SubCategoryModel
    {
        public SubCategoryModel()
        {

        }
        public SubCategoryModel(string name)
        {
            Name = name;
        }

        public SubCategoryModel(string name, string categoryName)
        {
            Name = name;
            CategoryName = categoryName;
        }

        public SubCategoryModel(int id, string name, string categoryName)
        {
            Id = id;
            Name = name;
            CategoryName = categoryName;
        }

        public int Id { get; set; }
        [Display(Name = "Under Kategori")]
        public string Name { get; set; }
        [Display(Name = "Kategori")]
        [ValidateNever]
        public string CategoryName { get; set; }    
    }
}
