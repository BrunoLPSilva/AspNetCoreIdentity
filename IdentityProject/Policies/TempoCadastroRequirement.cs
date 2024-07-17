using Microsoft.AspNetCore.Authorization;

namespace IdentityProject.Policies
{
    public class TempoCadastroRequirement : IAuthorizationRequirement
    {
        public int TempoCadastroMinimo { get; }

        public TempoCadastroRequirement(int tempoCadastroMinimo)
        {
            TempoCadastroMinimo = tempoCadastroMinimo;
        }
    }

}
