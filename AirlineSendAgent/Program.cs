// See https://aka.ms/new-console-template for more information
using AirlineSendAgent.App;
using AirlineSendAgent.Client;
using AirlineWeb.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.WriteLine("Hello, World!");


var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton<IAppHost, AppHost>();
        services.AddSingleton<IWebhookClient, WebhookClient>();
        services.AddDbContext<SendAgentDbContext>(o =>
        o.UseSqlServer(context.Configuration.GetConnectionString("DefaultConnection"))
        );
        services.AddHttpClient();

    })
    .Build();

host.Services.GetService<IAppHost>().Run();
