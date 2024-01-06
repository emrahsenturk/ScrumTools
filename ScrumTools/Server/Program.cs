using Blazored.Toast;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.ResponseCompression;
using ScrumTools.Server.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.AddServerSideBlazor();
builder.Services.AddBlazoredToast();

builder.Services.AddSingleton<IToastService, ToastService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapHub<ScrumPokerHub>("/scrumPokerHub");

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();

