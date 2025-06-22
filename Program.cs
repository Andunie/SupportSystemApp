using Microsoft.EntityFrameworkCore;
using SupportSystem.Infrastructure.Data;
using SupportSystem.Infrastructure.Repositories;
using SupportSystem.Application.Interfaces;
using SupportSystem.Application.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using SupportSystemApp.Application.Services;
using SupportSystemApp.Hubs;
using SupportSystemApp.Application.Interfaces;
using SupportSystemApp.Infrastructre.Repositories;
using SupportSystemApp.Infrastructre.SignalR;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

// 1. DbContext (appsettings.json içindeki "DefaultConnection" kullanılacak)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Repository DI
builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddScoped<PasswordHasherService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<MessageService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();

builder.Services.AddHttpClient<GeminiAIService>();
builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<EmailService>();

// 3. Razor Pages
builder.Services.AddRazorPages();

// SignalR
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
    options.MaximumReceiveMessageSize = 102400; // 100 KB
    options.StreamBufferCapacity = 10;
    options.HandshakeTimeout = TimeSpan.FromSeconds(15);
    options.KeepAliveInterval = TimeSpan.FromSeconds(10);
    options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
});

// 4. Oturum Yönetimi
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<CookieAuthService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.LoginPath = "/Login";
        options.LogoutPath = "/Logout";
        options.AccessDeniedPath = "/AccessDenied";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
    });

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.MapHub<NotificationHub>("/notificationhub");
app.MapHub<TicketChatHub>("/ticketchathub");

app.Run();