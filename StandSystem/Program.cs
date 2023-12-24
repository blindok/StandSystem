using Microsoft.EntityFrameworkCore;
using StandSystem.DataAccess;
using Microsoft.AspNetCore.Identity;
using StandSystem.Models;
using StandSystem.Authentication;
using StandSystem.IdentityScheme;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddIdentity<User, Role>()
        .AddDefaultTokenProviders();
builder.Services.AddTransient<IUserStore<User>, UserStore>();
builder.Services.AddTransient<IRoleStore<Role>, RoleStore>();


/*builder.Services.AddDefaultIdentity<IdentityUser>(
    options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();*/

builder.Services.AddSingleton<IDeviceManager, DeviceManager>();

builder.Services.AddSingleton<ShhKeyAuthorizationFilter>();
builder.Services.AddSingleton<ISshKeyValidator, SshKeyValidator>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
