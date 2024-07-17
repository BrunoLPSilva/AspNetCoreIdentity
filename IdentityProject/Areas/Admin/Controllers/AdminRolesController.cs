using IdentityProject.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace IdentityProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminRolesController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public AdminRolesController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public ViewResult Index() => View(roleManager.Roles);

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create([Required] string name)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    Errors(result);
                }
            }

            return View(name);
        }

        [HttpGet]
        public async Task<IActionResult> Update(string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Role ID is null or empty");
            }

            IdentityRole role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return NotFound($"Role with ID {id} not found");
            }

            List<IdentityUser> members = new List<IdentityUser>();
            List<IdentityUser> nonMembers = new List<IdentityUser>();

            foreach (IdentityUser user in userManager.Users.ToList())
            {
                var list = await userManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;
                list.Add(user);
            }

            return View(new RoleEdit
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            });
        }

        [HttpPost]
        public async Task<IActionResult> Update(RoleModification model)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result;

                foreach (string userId in model.AddIds ?? new string[] { })
                {
                    IdentityUser user = await userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        result = await userManager.AddToRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                        {
                            Errors(result);
                        }
                    }
                }

                foreach (string userId in model.DeleteIds ?? new string[] { })
                {
                    IdentityUser user = await userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        result = await userManager.RemoveFromRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                        {
                            Errors(result);
                        }
                    }
                }

                if (ModelState.IsValid)
                {
                    var currentUser = await userManager.GetUserAsync(User);
                    if (currentUser != null && (model.AddIds.Contains(currentUser.Id) || model.DeleteIds.Contains(currentUser.Id)))
                    {
                        await UpdateSecurityStampAsync(currentUser);
                    }

                    return RedirectToAction(nameof(Index));
                }
            }

            return await Update(model.RoleId);
        }

        private async Task UpdateSecurityStampAsync(IdentityUser user)
        {
            await userManager.UpdateSecurityStampAsync(user);
            await signInManager.SignOutAsync();
            await signInManager.SignInAsync(user, isPersistent: false);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ModelState.AddModelError("", "Role não encontrada");
                return View("Index", roleManager.Roles);
            }

            return View(role);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);

            if (role != null)
            {
                IdentityResult result = await roleManager.DeleteAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    Errors(result);
                }
            }

            return View(role);
        }

        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }
    }
}
