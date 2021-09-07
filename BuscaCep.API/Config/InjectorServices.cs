using BuscaCep.API.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BuscaCep.API.Config
{
    public static class InjectorServices
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<ICepService, CepService>();
        }
    }
}