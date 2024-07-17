using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace IdentityProject.Services
{
    public class SeedUserRoleInitial : ISeedUserRoleInitial
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SeedUserRoleInitial(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedRolesAsync()
        {
            if (!await _roleManager.RoleExistsAsync("User"))
            {
                IdentityRole role = new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "USER",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                };

                IdentityResult roleResult = await _roleManager.CreateAsync(role);
            }

            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                IdentityRole role = new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                };
                IdentityResult roleResult = await _roleManager.CreateAsync(role);
            }

            if (!await _roleManager.RoleExistsAsync("Gerente"))
            {
                IdentityRole role = new IdentityRole
                {
                    Name = "Gerente",
                    NormalizedName = "GERENTE",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                };
                IdentityResult roleResult = await _roleManager.CreateAsync(role);
            }
        }

        public async Task SeedUsersAsync()
        {
            if (await _userManager.FindByEmailAsync("usuario@localhost") == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = "usuario@localhost",
                    Email = "usuario@localhost",
                    NormalizedUserName = "USUARIO@LOCALHOST",
                    NormalizedEmail = "USUARIO@LOCALHOST",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                IdentityResult result = await _userManager.CreateAsync(user, "Numsey#2023");

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                }
            }

            if (await _userManager.FindByEmailAsync("admin@localhost") == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = "admin@localhost",
                    Email = "admin@localhost",
                    NormalizedUserName = "ADMIN@LOCALHOST",
                    NormalizedEmail = "ADMIN@LOCALHOST",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                IdentityResult result = await _userManager.CreateAsync(user, "Numsey#2023");

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                }
            }

            if (await _userManager.FindByEmailAsync("gerente@localhost") == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = "gerente@localhost",
                    Email = "gerente@localhost",
                    NormalizedUserName = "GERENTE@LOCALHOST",
                    NormalizedEmail = "GERENTE@LOCALHOST",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                IdentityResult result = await _userManager.CreateAsync(user, "Numsey#2023");

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Gerente");
                }
            }
        }
    }
}
