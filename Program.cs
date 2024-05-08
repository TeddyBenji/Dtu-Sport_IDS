using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Dtu_Sport_IDS.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), 
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policyBuilder =>
        {
            policyBuilder.WithOrigins("http://localhost:5242") // Client application URL
                         .AllowAnyHeader()
                         .AllowAnyMethod()
                         .AllowCredentials(); // Allowing credentials is optional based on your security requirements
        });
});






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
    .AddAspNetIdentity<IdentityUser>()
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddInMemoryIdentityResources(Config.IdentityResources)  // Ensure this line is correctly added
    .AddInMemoryApiResources(Config.ApiResources)
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
app.UseCors(builder =>
    builder.WithOrigins("https://localhost:7033")
           .AllowAnyMethod()
           .AllowAnyHeader()
           .AllowCredentials());

app.UseAuthentication();
app.UseIdentityServer();
app.UseAuthorization();

app.MapControllers();
// Seed roles
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await RolesDataInitializer.SeedRoles(roleManager);
}

app.Run();




