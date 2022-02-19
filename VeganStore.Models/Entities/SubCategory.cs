using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeganStore.Models.Entities
{
    public class SubCategory
    {
        public SubCategory()
        {

        }

        public SubCategory(string name, int categoryId)
        {
            Name = name;
            CategoryId = categoryId;
        }

        public SubCategory(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public SubCategory(int id, string name, int categoryId)
        {
            Id = id;
            Name = name;
            CategoryId = categoryId;
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
