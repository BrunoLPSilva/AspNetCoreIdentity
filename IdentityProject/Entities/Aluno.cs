using System.ComponentModel.DataAnnotations;

namespace IdentityProject.Entities
{
    public class Aluno
    {
        public int AlunoId { get; set; }

        [Required, MaxLength(80, ErrorMessage = "Nome não pode exceder 80 caracteres")]
        public string Nome { get; set; }

        [Required, EmailAddress, MaxLength(120, ErrorMessage = "Email não pode exceder 120 caracteres")]
        public string Email { get; set; }

        public int Idade { get; set; }

        [MaxLength(80, ErrorMessage = "Curso não pode exceder 80 caracteres")]
        public string? Curso { get; set; }
    }
}
