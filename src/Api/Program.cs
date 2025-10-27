
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using challenge.Api.Swagger;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Reflection;
using Infrastructure.Extensions;
using challenge.Infrastructure.Repositories;
using challenge.Domain.Interfaces;
using challenge.Application.Services;
using MongoDB.Driver;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
}).AddMvc();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Challenge Mottu API V1",
        Version = "v1",
        Description = "API para gerenciamento de veículos e usuários - Versão 1",
        Contact = new OpenApiContact
        {
            Name = "Equipe Challenge",
            Email = "challenge@mottu.com"
        }
    });

    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Title = "Challenge Mottu API V2",
        Version = "v2",
        Description = "API para gerenciamento de veículos e usuários - Versão 2 (Exemplo de versionamento)",
        Contact = new OpenApiContact
        {
            Name = "Equipe Challenge",
            Email = "challenge@mottu.com"
        }
    });

    // Filtro para garantir que o Swagger separe as versões
    options.OperationFilter<RemoveVersionParameters>();
    options.DocumentFilter<ReplaceVersionWithExactValueInPath>();

    

    // Include XML comments
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

// Database configuration
builder.Services.AddMongoDb(builder.Configuration);


// Repositories are now handled by IMongoRepository in Infrastructure.Extensions.MongoDbExtensions

// Service registration
builder.Services.AddScoped<VehicleService>();
builder.Services.AddScoped<UserService>();

// Health Check Configuration
builder.Services.AddHealthChecks()
    .AddMongoDb(sp => sp.GetRequiredService<IMongoClient>(), name: "mongodb", failureStatus: HealthStatus.Unhealthy, tags: new[] { "ready" });

// CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Challenge Mottu API v1");
        options.SwaggerEndpoint("/swagger/v2/swagger.json", "Challenge Mottu API v2");
        options.RoutePrefix = string.Empty; // Swagger na raiz
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

// Health Check Endpoint
app.MapHealthChecks("/health", new HealthCheckOptions
{
    Predicate = healthCheck => healthCheck.Tags.Contains("ready"),
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var response = new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description
            })
        };
        await context.Response.WriteAsJsonAsync(response);
    }
});

app.MapControllers();

app.Run();

