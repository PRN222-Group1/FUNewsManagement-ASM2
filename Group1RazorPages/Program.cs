using DataAccessLayer.Data;
using Group1RazorPages.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Set default page to be login page
builder.Services.AddRazorPages()
    .AddRazorPagesOptions(opt =>
    {
        opt.Conventions.AddPageRoute("/Account/Login", "");
    });

// Add services to the container.
builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

// Create a scope and call the service manually
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var dbContext = services.GetRequiredService<FuNewsManagementContext>();
var logger = services.GetRequiredService<ILogger<Program>>();

try
{
    // Migrate changes to the database
    await dbContext.Database.MigrateAsync();

    // Seed the database with data
    await FuNewsManagementContextSeed.SeedAsync(dbContext);
}
catch (Exception ex)
{
    logger.LogError(ex, "A message occured during migration");
}


app.Run();
