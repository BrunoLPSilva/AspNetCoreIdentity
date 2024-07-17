using Microsoft.AspNetCore.Authorization;
using System.Net;

//namespace IdentityProject.Policies
//{
//    public class TempoCadastroHandler : AuthorizationHandler<TempoCadastroRequirement>
//    {
//        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
//            TempoCadastroRequirement requirement)
//        {

            //if (context.User.HasClaim(c => c.Type == "CadastradoEm"))
            //{
            //    var data = context.User.FindFirst(c => c.Type == "CadastradoEm").Value;

            //    var dataCadastro = DateTime.Parse(data);

            //    double tempoCadastro = await Task.Run(() =>
            //                    (DateTime.Now.Date - dataCadastro.Date).TotalDays);

            //    var tempoEmAnos = tempoCadastro / 360;

            //    if (tempoEmAnos >= requirement.TempoCadastroMinimo)
            //    {
            //        context.Succeed(requirement);
            //    }
            //    return;
            //}


namespace IdentityProject.Policies
    {
        public class TempoCadastroHandler : AuthorizationHandler<TempoCadastroRequirement>
        {
            protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TempoCadastroRequirement requirement)
            {
                // Verifique se o usuário tem a claim "CadastradoEm"
                var cadastradoEmClaim = context.User.Claims.FirstOrDefault(c => c.Type == "CadastradoEm");

                if (cadastradoEmClaim != null && DateTime.TryParse(cadastradoEmClaim.Value, out var dataCadastro))
                {
                    var tempoCadastro = DateTime.Now - dataCadastro;

                    if (tempoCadastro.Days >= requirement.TempoCadastroMinimo)
                    {
                        // Requisito atendido
                        context.Succeed(requirement);
                    }
                }

                // Requisito não atendido
                return Task.CompletedTask;
            }
        }
 }


    




