using System.ComponentModel.DataAnnotations;

namespace IdentityProject.Areas.Admin.Models
{
    public class RoleModification
    {
        [Required]
        public string? RoleName { get; set; }
        public string? RoleId { get; set; }

        public string[] AddIds { get; set; } = Array.Empty<string>(); // Inicializando para evitar null
        public string[] DeleteIds { get; set; } = Array.Empty<string>(); // Inicializando para evitar null
    }
}
