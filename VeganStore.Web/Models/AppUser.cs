using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace VeganStore.Web.Models
{
    public class AppUser : IdentityUser
    {        
        [PersonalData]
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [PersonalData]
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [PersonalData]
        [Required]
        [StringLength(50)]
        public string StreetAddress { get; set; }

        [PersonalData]
        [Required]
        [StringLength(50)]
        public string PostalCode { get; set; }

        [PersonalData]
        [Required]
        [StringLength(50)]
        public string City { get; set; }
       
    }
}
