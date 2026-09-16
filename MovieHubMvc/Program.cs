using MovieHubMvc.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// ===== Email configuration from appsettings.json =====
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

// ===== DI lifetime explanation (assignment requirement) =====
//
// Transient  – новий екземпляр КОЖНОГО разу при запиті сервісу.
//              Підходить для легких, без стану об’єктів.
//
// Scoped     – один екземпляр на HTTP-запит (scope).
//              Рекомендовано для сервісів, що працюють з БД / HTTP-контекстом.
//              EmailSender реєструємо як Scoped — один раз на submit форми.
//
// Singleton  – один екземпляр на весь час життя додатку.
//              Небезпечно, якщо сервіс тримає стан або не thread-safe.
//
// Обираємо Scoped для IEmailSender:
builder.Services.AddScoped<IEmailSender, EmailSender>();

// Альтернативи (закоментовано для демонстрації):
// builder.Services.AddTransient<IEmailSender, EmailSender>();
// builder.Services.AddSingleton<IEmailSender, EmailSender>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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
