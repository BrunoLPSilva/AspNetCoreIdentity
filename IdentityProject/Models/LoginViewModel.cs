using System.ComponentModel.DataAnnotations;

namespace IdentityProject.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "O email é obrigatorio")]
        [EmailAddress(ErrorMessage = "Email invalido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatoria")]
        public string Password { get; set; }

        [Display(Name ="Lembrar-me")]
        public bool RememberMe { get; set; }
    }
}
