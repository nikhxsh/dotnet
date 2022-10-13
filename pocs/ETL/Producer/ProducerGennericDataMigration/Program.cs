using ProducerGennericDataMigration;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<SQL_Setting>();
        services.AddSingleton<SQS_Setting>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();