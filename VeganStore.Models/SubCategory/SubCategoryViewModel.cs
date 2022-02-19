using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeganStore.Models.SubCategory
{
    public class SubCategoryViewModel
    {
        public SubCategoryViewModel(string name, string categoryName)
        {
            Name = name;
            CategoryName = categoryName;
        }
        [Required]
        [Display(Name = "Sub Category")]
        public string Name { get; set; }
        [Required]

        [Display(Name = "Category")]
        public string CategoryName { get; set; }
    }
}
