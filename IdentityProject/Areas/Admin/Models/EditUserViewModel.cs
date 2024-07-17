using System.Security.Claims;

namespace IdentityProject.Areas.Admin.Models;

public class EditUserViewModel
{
    public EditUserViewModel()
    {
        Claims = new List<Claim>();
    }
    public string? Id { get; set; }

    public string? UserName { get; set; }
    public string? Email { get; set; }
    public List<Claim>? Claims { get; set; }
}
