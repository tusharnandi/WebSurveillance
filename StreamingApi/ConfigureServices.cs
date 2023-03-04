using Microsoft.Net.Http.Headers;

namespace VideoStreamApi
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddPresentationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient();
            services.AddOptions();

            string MyAllowSpecificOrigins = "AllowStreamServerOrigins";

            var allowOrigins = configuration.GetValue<string>("AllowOrigins"); 

            services.AddCors(o => o.AddPolicy(MyAllowSpecificOrigins,
                      builder =>
                      {
                          builder.WithOrigins(allowOrigins)
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                      }));
            //builder.AllowAnyOrigin()
            //builder.WithOrigins(allowOrigins)
            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            return services;
        }
    }
}
