using System.ComponentModel.DataAnnotations;

namespace VeganStore.Web.Models.ViewModels
{
    public class SignInViewModel
    {
        [Display(Name = "E-postadress")]
        [EmailAddress(ErrorMessage = "E-postadressen måste vara en giltig e-postadress")]
        [Required(ErrorMessage = "Du måste ange en e-postadress")]
        [StringLength(100, ErrorMessage = "E-postadressen måste vara en giltig e-postadress", MinimumLength = 6)]
        public string Email { get; set; }

        [Display(Name = "Lösenord")]
        [Required(ErrorMessage = "Du måste ange ett lösenord")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Lösenordet måste bestå av minst 8 tecken", MinimumLength = 8)]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}
