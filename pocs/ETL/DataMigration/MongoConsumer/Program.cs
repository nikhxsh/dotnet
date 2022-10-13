using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SqsService;
using SqlService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SqlService.Helper;
using PostgresService;
using MongoConsumer.MongoService;

namespace MongoConsumer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<PollMessage>();
                    services.AddSingleton<SqlClientService>();
                    services.AddSingleton<PostgresClientService>();
                    services.AddSingleton<ConsumerService>();
                    services.AddSingleton<MongoRepository>();
                    services.AddHostedService<Worker>();
                });
    }
}
