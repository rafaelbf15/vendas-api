using Vendas.API.Configuration;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using Serilog;
using Vendas.Domain.Models;
using Vendas.Infra.Context;
using System.Linq;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/vendas.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddApiConfiguration(builder.Configuration);

builder.Services.RegisterServices();

builder.Services.AddSwaggerConfiguration(builder.Environment);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<VendasDbContext>();
    context.Database.MigrateAsync().GetAwaiter().GetResult();   

    if (!context.Filiais.Any())
        context.Filiais.Add(new Filial { Id = new Guid("08703488-e04f-4cf6-ab1a-95bb6e453a55"), Nome = "Filial Teste" });
    if (!context.Clientes.Any())
        context.Clientes.Add(new Cliente { Id = new Guid("2ee19143-4d46-42ad-ba1e-2dad99625c8c"), Nome = "Cliente Teste" });
    if (!context.Produtos.Any())
        context.Produtos.Add(new Produto { Id = new Guid("c873c887-6c7a-41ca-a060-6ac4b4a2aa3f"), Nome = "Produto Teste" });

    context.SaveChanges();
}

var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

app.UseDeveloperExceptionPage();

app.UseSwaggerConfiguration(apiVersionDescriptionProvider);

app.UseApiConfiguration(app.Environment, builder.Configuration);

app.Run();

public partial class Program { }

