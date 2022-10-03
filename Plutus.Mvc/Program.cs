using Microsoft.EntityFrameworkCore;
using MVC.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<PlutusDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetValue<string>("NpgSqlConnectionString")));

builder.Services.AddTransient<PlutusDbContext>();

builder.Services.AddSingleton(provider =>
{
    using var scope = provider.CreateScope();
    var context =  scope.ServiceProvider.GetService<PlutusDbContext>();
    return context!.Categories.OrderBy(c => c.Name).ToArray();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();