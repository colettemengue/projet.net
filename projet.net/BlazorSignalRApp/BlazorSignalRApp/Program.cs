using BlazorSignalRApp.Client.Pages;
using BlazorSignalRApp.Components;
using Microsoft.AspNetCore.ResponseCompression;
using BlazorSignalRApp.Hubs;
using BlazorSignalRApp.data;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;



var builder = WebApplication.CreateBuilder(args);


/*var logPath = Path.GetFullPath("logs/app.log");
Console.WriteLine($"Chemin des logs: {logPath}"); */

// file creation
var logPath = "C:/Users/colet/source/repos/projet.net/BlazorSignalRApp/bin/Debug/net9.0/app.log";
var logDir = Path.GetDirectoryName(logPath);

if (!string.IsNullOrEmpty(logDir) && !Directory.Exists(logDir))
{
    Directory.CreateDirectory(logDir);
    Console.WriteLine($"Dossier de logs créé : {logDir}");
}


builder.Host.UseNLog(); // Charge NLog APRÈS avoir vérifié le dossier

//enabe NLog
builder.Host.UseNLog(); // NLog: Setup NLog for Dependency injection it's me 

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddSignalR();
builder.Services.AddControllers();

builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        ["application/octet-stream"]);
});
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000);
});
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=messages.db"));

var app = builder.Build();
app.UseResponseCompression();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(BlazorSignalRApp.Client._Imports).Assembly);
app.MapHub<ChatHub>("/chathub");
app.MapControllers();


app.Run();
