using NotificationService;
using NotificationService.Service;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddSingleton<IEmailService, EmailService>();

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton<IEmailService, EmailService>();
        services.AddHostedService<Worker>();
    })
    .Build();
await host.RunAsync();
