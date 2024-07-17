namespace IdentityProject.Services
{
    public interface ISeedUserRoleInitial
    {
        Task SeedRolesAsync();
        Task SeedUsersAsync();

    }
}
