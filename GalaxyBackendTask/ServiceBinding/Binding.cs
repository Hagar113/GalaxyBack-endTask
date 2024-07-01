using DataProvider.Provider;
using DataProvider.IProvider;

namespace GalaxyBackendTask.ServiceBinding
{
    public static class Binding
    {
        public static IServiceCollection InjectServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthProvider, AuthProvider>();


            return services;
        }
    }
}
