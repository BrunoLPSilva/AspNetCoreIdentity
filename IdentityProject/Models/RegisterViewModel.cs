using System.ComponentModel.DataAnnotations;

namespace IdentityProject.Models
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirme a senha")]
        [Compare("Password", ErrorMessage ="As senhas não são iguais")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Lembrar-me")]
        public bool RememberMe { get; set; }

    }
}
