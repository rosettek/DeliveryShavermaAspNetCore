using BarsGroupProjectN1.Core.AppSettings;
using Handler.Core.Common;
using HandlerService.Extensions;
using HandlerService.Middlewares;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDependencies();

builder.Services.Configure<ServicesOptions>(builder.Configuration.GetSection("Services"));

builder.Services.AddServicesHttpClients(builder.Configuration.GetSection("Services").Get<ServicesOptions>());

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.SetCorsPolicies(app.Services.GetService<IOptions<ServicesOptions>>()!);

app.UseStaticFiles();

app.UseRouting();

app.UseMiddleware<UserMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Payment}/{action=Index}/{id?}");


app.Run();