//using IdentityProject.Context;
//using IdentityProject.Services;
//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;



//var builder = WebApplication.CreateBuilder(args);

//var connection = builder.Configuration.GetConnectionString("DefaultConnection");

//builder.Services.AddDbContext<AppDbContext>(options =>
//options.UseSqlServer(connection));

//builder.Services.AddIdentity<IdentityUser,IdentityRole>().AddEntityFrameworkStores<AppDbContext>();


////Padrao de complecidade de senha
//builder.Services.Configure<IdentityOptions>(options =>
//{
//    options.Password.RequiredLength = 10;
//    options.Password.RequiredUniqueChars = 3;
//    options.Password.RequireNonAlphanumeric = false;
//});


////Configura propriedades dos esquema de autenticacao baseados em cookie
//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>{
//    options.Cookie.Name = "AspNetCore.cookies";
//    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
//    options.SlidingExpiration = true;

//});

//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("RequireUserAdminGerenteRole",
//        policy => policy.RequireRole("User", "Admin", "Gerente"));


//});

//builder.Services.AddControllersWithViews();

//builder.Services.AddScoped<ISeedUserRoleInitial, SeedUserRoleInitial>();


//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}


//app.UseHttpsRedirection();
//app.UseStaticFiles();

//app.UseRouting();


//await CriarPerfisUsuariosAsync(app);
//app.UseAuthentication();
//app.UseAuthorization();

//app.MapControllerRoute(
//    name: "MinhaArea",
//    pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}");


//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.Run();


//async Task CriarPerfisUsuariosAsync(WebApplication app)
//{
//    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
//    using(var scoped = scopedFactory.CreateScope()) { 

//        var service = scoped.ServiceProvider.GetService<ISeedUserRoleInitial>();
//        await service.SeedRolesAsync();
//        await service.SeedUsersAsync();
//    }
//}

using IdentityProject.Context;
using IdentityProject.Policies;
using IdentityProject.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configuração da string de conexão
var connection = builder.Configuration.GetConnectionString("DefaultConnection");

// Configuração do DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connection));

// Configuração do Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

// Configuração das opções de senha
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 10;
    options.Password.RequiredUniqueChars = 3;
    options.Password.RequireNonAlphanumeric = false;
});

// Configuração da autenticação
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "AspNetCore.cookies";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
        options.SlidingExpiration = true;
    });

// Configuração da autorização
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireUserAdminGerenteRole",
        policy => policy.RequireRole("User", "Admin", "Gerente"));
});


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("IsAdminClaimAccess",
        policy => policy.RequireClaim("CadastradoEm"));

    options.AddPolicy("IsAdminClaimAccess",
        policy => policy.RequireClaim("IsAdmin", "true"));

    options.AddPolicy("IsFuncionarioClaimAccess",
       policy => policy.RequireClaim("IsFuncionario", "true"));

    options.AddPolicy("TempoCadastroMinimo", policy =>
    {
        policy.Requirements.Add(new TempoCadastroRequirement(365));
    });
});


builder.Services.AddControllersWithViews();

//register service
builder.Services.AddScoped<IAuthorizationHandler, TempoCadastroHandler>();

builder.Services.AddScoped<ISeedUserRoleInitial, SeedUserRoleInitial>();
builder.Services.AddScoped<ISeedUserClaimsInitial, SeedUserClaimsInitial>();



var app = builder.Build();

// Configuração do pipeline de requisições
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
await CriarPerfisUsuariosAsync(app);
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "MinhaArea",
    pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

async Task CriarPerfisUsuariosAsync(WebApplication app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
    using (var scoped = scopedFactory.CreateScope())
    {
        //var service = scoped.ServiceProvider.GetService<ISeedUserRoleInitial>();
        //await service.SeedRolesAsync();
        //await service.SeedUsersAsync();

        var service = scoped.ServiceProvider.GetService<ISeedUserClaimsInitial>();
        await service.SeedUserClaims();
    }
}
