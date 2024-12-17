using BuildingBlocks.Interfaces;
using BuildingBlocks.Services;
using Escs_Client.Helpers;
using Escs_Client.Interfaces;
using Escs_Client.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient("ApiClient")
        .AddHttpMessageHandler<RefreshTokenHandler>();
builder.Services.AddScoped<RefreshTokenHandler>();
builder.Services.AddTransient<IHttpCaller, HttpCaller>();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Authentication/Login";
        options.LogoutPath = "/Authentication/Logout";
        options.AccessDeniedPath = "/Authentication/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
    });


builder.Services.AddTransient<IAuthenService, AuthenService>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<IKeyService, KeyService>();
builder.Services.AddTransient<IEndpointService, EndpointService>();

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

app.UseAuthentication();

app.UseAuthorization();

app.MapAreaControllerRoute(
    name: "AdminRouting",
    areaName: "Administration",
    pattern: "Administration/{controller=Home}/{action=Index}/{id?}"
    );


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
