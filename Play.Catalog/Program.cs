
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Play.Catalog.Entities;
using Play.Catalog.Repo;
using Play.Catalog.Settings;
using System.Collections;

namespace Play.Catalog
{
    public class Program
    {
        private static ServiceSettings serviceSettings;

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));


            serviceSettings = builder.Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();

            builder.Services.AddSingleton(serviceProvider =>
            {
                var mongoDbSettings = builder.Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
                var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
                return mongoClient.GetDatabase(serviceSettings.ServiceName);

            });

            builder.Services.AddSingleton<IItemRepo, ItemRepo>();

            builder.Services.AddControllers(
                options =>
                {
                    options.SuppressAsyncSuffixInActionNames = false;
                });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}