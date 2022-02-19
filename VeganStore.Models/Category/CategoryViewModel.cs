using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeganStore.Models.Category
{
    public class CategoryViewModel
    {
        public CategoryViewModel(string name)
        {
            Name = name;
        }
        [Required]
        public string Name { get; set; }
    }
}
