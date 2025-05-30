using Domain.Interfaces;
using Repository;
using Services.Account;
using Services.Home;

namespace AuthenticatorAppNew.Extension
{
    public static class ServiceExtension
    {

        public static IServiceCollection AddServices(this IServiceCollection services)
        {   
            return services.AddScoped<AccountServices>()
                .AddScoped<HomeServices>();
            
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services
                .AddScoped(typeof(IRepository<>), typeof(Repositoryy<>));
        }


    }
}
