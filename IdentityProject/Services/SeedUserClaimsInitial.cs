//using Microsoft.AspNetCore.Identity;
//using System.Security.Claims;

//namespace IdentityProject.Services
//{
//    public class SeedUserClaimsInitial : ISeedUserClaimsInitial
//    {
//        private readonly UserManager<IdentityUser> _userManager;
//        public SeedUserClaimsInitial(UserManager<IdentityUser> userManager)
//        {
//            _userManager = userManager;
//        }

//        public async Task SeedUserClaims()
//        {
//            try
//            {
//                IdentityUser user1 = await _userManager.FindByEmailAsync("gerente@localhost");

//                if (user1 is not null)
//                {
//                    var claimList = (await _userManager.GetClaimsAsync(user1)).Select(p => p.Type);
//                    if (!claimList.Contains("CadastradoEm"))
//                    {
//                        var claimResult1 = await _userManager.AddClaimAsync(user1, new Claim("CadastradoEm", "03/03/2021"));
//                    }
//                }

//                //usuario2
//                IdentityUser user2 = await _userManager.FindByEmailAsync("usuario@localhost");

//                if (user2 is not null)
//                {
//                    var claimList = (await _userManager.GetClaimsAsync(user2)).Select(p => p.Type);
//                    if (!claimList.Contains("CadastradoEm"))
//                    {
//                        var claimResult1 = await _userManager.AddClaimAsync(user2, new Claim("CadastradoEm", "01/01/2021"));
//                    }
//                }



//                //usuario3
//                IdentityUser user3 = await _userManager.FindByEmailAsync("bruninho@testando.com");

//                if (user3 is not null)
//                {
//                    var claimList = (await _userManager.GetClaimsAsync(user3)).Select(p => p.Type);
//                    if (!claimList.Contains("CadastradoEm"))
//                    {
//                        var claimResult1 = await _userManager.AddClaimAsync(user3, new Claim("CadastradoEm", "01/01/2021"));
//                    }
//                }

//                IdentityUser user4 = await _userManager.FindByEmailAsync("admin@localhost");
//                if (user4 is not null)
//                {
//                    var claimList = (await _userManager.GetClaimsAsync(user4)).Select(p => p.Type);

//                    if (!claimList.Contains("CadastradoEm"))
//                    {
//                        var claimResult = await _userManager.AddClaimAsync(user4,
//                                                                    new Claim("CadastradoEm", "15/09/2014"));
//                    }

//                    if (!claimList.Contains("IsAdmin"))
//                    {
//                        var claimResult2 = await _userManager.AddClaimAsync(user4, new Claim("IsAdmin", "true"));
//                    }
//                }

//                //IdentityUser user2 = await _userManager.FindByEmailAsync("usuario@localhost");
//                //if (user1 is not null)
//                //{
//                //    var claimList = (await _userManager.GetClaimsAsync(user1)).Select(p => p.Type);

//                //    if (!claimList.Contains("IsAdmin"))
//                //    {
//                //        var claimResult = await _userManager.AddClaimAsync(user1,
//                //                                                    new Claim("IsAdmin", "false"));
//                //    }

//                //    if (!claimList.Contains("IsFuncionario"))
//                //    {
//                //        var claimResult2 = await _userManager.AddClaimAsync(user1, new Claim("IsFuncionario", "true"));
//                //    }
//                //}
//            }
//            catch (Exception)
//            {
//                throw;
//            }
//        }
//    }
//}

using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityProject.Services
{
    public class SeedUserClaimsInitial : ISeedUserClaimsInitial
    {
        private readonly UserManager<IdentityUser> _userManager;

        public SeedUserClaimsInitial(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task SeedUserClaims()
        {
            try
            {
                // Lista de usuários e suas claims
                var usersWithClaims = new[]
                {
                    new { Email = "gerente@localhost", ClaimType = "CadastradoEm", ClaimValue = "03/03/2021" },
                    new { Email = "usuario@localhost", ClaimType = "CadastradoEm", ClaimValue = "01/01/2021" },
                    new { Email = "bruninho@testando.com", ClaimType = "CadastradoEm", ClaimValue = "01/01/2021" },
                    new { Email = "admin@localhost", ClaimType = "CadastradoEm", ClaimValue = "15/09/2014" },
                    new { Email = "admin@localhost", ClaimType = "IsAdmin", ClaimValue = "true" }
                };

                foreach (var userClaim in usersWithClaims)
                {
                    var user = await _userManager.FindByEmailAsync(userClaim.Email);
                    if (user != null)
                    {
                        var claims = await _userManager.GetClaimsAsync(user);
                        if (!claims.Any(c => c.Type == userClaim.ClaimType))
                        {
                            var result = await _userManager.AddClaimAsync(user, new Claim(userClaim.ClaimType, userClaim.ClaimValue));
                            if (!result.Succeeded)
                            {
                                // Log or handle the error based on your application's logging framework
                                throw new Exception($"Failed to add claim {userClaim.ClaimType} to user {userClaim.Email}.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception (or handle it appropriately)
                // Example: Console.WriteLine(ex.Message);
                throw new Exception("An error occurred while seeding user claims.", ex);
            }
        }
    }
}

