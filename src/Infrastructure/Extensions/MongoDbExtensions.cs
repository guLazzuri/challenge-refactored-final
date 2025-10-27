using challenge.Domain.Interfaces;
using Infrastructure.Data;
using challenge.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Infrastructure.Extensions
{
    public static class MongoDbExtensions
    {
        public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            // 1. Configurar as Settings
            var mongoDbSettings = configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();
            if (mongoDbSettings == null)
            {
                throw new InvalidOperationException("MongoDbSettings não encontrado na configuração.");
            }
            services.AddSingleton(mongoDbSettings);

            // 2. Configurar o MongoClient e a Database (opcional, mas bom para reuso)
            services.AddSingleton<IMongoClient>(serviceProvider =>
            {
                return new MongoClient(mongoDbSettings.ConnectionString);
            });

            services.AddSingleton(serviceProvider =>
            {
                var client = serviceProvider.GetRequiredService<IMongoClient>();
                return client.GetDatabase(mongoDbSettings.DatabaseName);
            });

            // 3. Registrar o repositório genérico
            services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));

            // 4. Registrar repositórios específicos
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            // services.AddScoped<IMaintenanceHistoryRepository, MaintenanceHistoryRepository>();

            return services;
        }
    }
}
