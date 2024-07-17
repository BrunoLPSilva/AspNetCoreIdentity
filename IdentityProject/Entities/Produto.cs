
using System.ComponentModel.DataAnnotations;


namespace IdentityProject.Entities
{
    public class Produto
    {
        public int ProdutoId { get; set; }
        [Required, MaxLength(80, ErrorMessage = "Não pode exceder")]
        public string? Nome { get; set; }
        public decimal Preco { get; set; }
    }
}
