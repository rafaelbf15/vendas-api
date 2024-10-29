using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using Vendas.Core.Options;
using Vendas.Domain.Models;
using Vendas.Infra.Context;
using Xunit;

namespace Vendas.Tests.Config
{
    public class VendasApiIntegrationTestFixture : WebApplicationFactory<Program>, IAsyncLifetime
    {
        public readonly Guid VendaId = Guid.NewGuid();
        public readonly Guid FilialId = new("08703488-e04f-4cf6-ab1a-95bb6e453a55");
        public readonly Guid ClienteId = new("2ee19143-4d46-42ad-ba1e-2dad99625c8c");
        public readonly Guid ProdutoId = new("c873c887-6c7a-41ca-a060-6ac4b4a2aa3f");

        private const string Database = "VendasDbTests";
        private const string Username = "root";
        private const string Password = "K86l0QK9EltKIjZ";
        private const ushort MySqlPort = 3306;

        private const ushort RabbitMqPort = 5672;
        private const string Host = "localhost";

        private IContainer _mysqlContainer;
        private IContainer _rabbitMqContainer;
        private WebApplicationFactory<Program> _factory;

        public async Task InitializeAsync()
        {

            _mysqlContainer = new ContainerBuilder()
                .WithImage("mysql:latest")
                .WithPortBinding(MySqlPort, true)
                .WithEnvironment("MYSQL_ROOT_PASSWORD", Password)
                .WithEnvironment("MYSQL_DATABASE", Database)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(MySqlPort))
                .Build();

            await _mysqlContainer.StartAsync();


            _rabbitMqContainer = new ContainerBuilder()
                .WithImage("rabbitmq:3-management")
                .WithPortBinding(5671, 5671)    
                .WithPortBinding(15671, 15671)     
                .WithEnvironment("RABBITMQ_DEFAULT_USER", "guest")
                .WithEnvironment("RABBITMQ_DEFAULT_PASS", "guest")
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5672))
                .Build();

            await _rabbitMqContainer.StartAsync();

            var host = _mysqlContainer.Hostname;
            var port = _mysqlContainer.GetMappedPublicPort(MySqlPort);
            var connectionString = $"Server={host};Port={port};Database={Database};Uid={Username};Pwd={Password};";


            _factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.AddDbContext<VendasDbContext>(options =>
                            options.UseMySQL(connectionString));

                        services.AddSingleton(new RabbitMQSettings
                        {
                            HostName = Host,
                            Port = RabbitMqPort,
                            UserName = "guest",
                            Password = "guest"
                        });

                        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
                    });
                });


            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<VendasDbContext>();
            dbContext.Database.Migrate();

            if (await dbContext.Database.CanConnectAsync())
            {
                if (!dbContext.Filiais.Any(f => f.Id == FilialId))
                    await dbContext.Filiais.AddAsync(new Filial { Id = FilialId, Nome = "Filial Teste" });

                if (!dbContext.Clientes.Any(c => c.Id == ClienteId))
                    await dbContext.Clientes.AddAsync(new Cliente { Id = ClienteId, Nome = "Cliente Teste" });

                if (!dbContext.Produtos.Any(p => p.Id == ProdutoId))
                    await dbContext.Produtos.AddAsync(new Produto { Id = ProdutoId, Nome = "Produto Teste" });

                await dbContext.SaveChangesAsync();
            }

        }

        public async Task DisposeAsync()
        {
            await _mysqlContainer.StopAsync();
            await _rabbitMqContainer.StopAsync();
            await _mysqlContainer.DisposeAsync();
            await _rabbitMqContainer.DisposeAsync();
            _factory?.Dispose();
        }
    }
}
