using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Dtu_Sport_IDS.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), 
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


// Explicitly add authorization services
builder.Services.AddAuthorization();

builder.Services.AddScoped<IUserService, UserService>();



// Add IdentityServer
builder.Services.AddIdentityServer(options =>
    {
        // IdentityServer options configuration
    })
    .AddAspNetIdentity<IdentityUser>() // Integrate ASP.NET Core Identity with IdentityServer
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddInMemoryClients(Config.Clients)
    .AddProfileService<UserProfileService>()
    .AddDeveloperSigningCredential();

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();
app.UseAuthentication(); // This will configure the app to use authentication
app.UseIdentityServer(); // This will configure the app to use IdentityServer
app.UseAuthorization(); // This must be called after UseRouting and before UseEndpoints

app.MapControllers();
// Seed roles
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await RolesDataInitializer.SeedRoles(roleManager);
}

app.Run();




