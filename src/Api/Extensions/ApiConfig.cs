using Microsoft.AspNetCore.Mvc;

namespace Api.Extensions
{
    public static class ApiConfig
    {
        public static IServiceCollection WebApiConfig(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddCors(options =>
            {
                options.AddPolicy("Development",
                    policy => policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    );
            });

            return services;
        }

        public static IApplicationBuilder UseMvcConfiguration(this IApplicationBuilder app)
        {
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseCors("Development");

            return app;
        }
    }
}
