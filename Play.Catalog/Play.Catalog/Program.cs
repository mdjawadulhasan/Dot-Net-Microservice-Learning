using Play.Catalog.Entities;
using Play.Common.MongoDB;
using Play.Common.Settings;

namespace Play.Catalog
{
    public class Program
    {
        private static ServiceSettings serviceSettings;

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            serviceSettings = builder.Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
            builder.Services.AddMongo().AddMongoRepository<Item>("items");
            builder.Services.AddMongo().AddMongoRepository<Item>("items");
            builder.Services.AddMongo().AddMongoRepository<Item>("items");
            builder.Services.AddMongo().AddMongoRepository<Item>("items");
            builder.Services.AddMongo().AddMongoRepository<Item>("items");

            builder.Services.AddControllers(
                options =>
                {
                    options.SuppressAsyncSuffixInActionNames = false;
                });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

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