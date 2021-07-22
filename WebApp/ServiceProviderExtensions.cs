using Microsoft.Extensions.DependencyInjection;
using DAL.EF;
using Service;
using Domain;
using Domain.Models;

namespace WebApp
{
    /// <summary>
    /// Extension method for adding custom services
    /// </summary>
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// Add services for working with entities
        /// </summary>
        /// <param name="services"></param>
        public static void AddEntityServices(this IServiceCollection services)
        {
            // add UoW
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // add repositories
            services.AddScoped<IGenericRepository<Artist>, GenericRepository<Artist>>();
            services.AddScoped(typeof(IGenericRepository<Address>), typeof(GenericRepository<Address>));
            services.AddScoped(typeof(IGenericRepository<AddressElement>), typeof(GenericRepository<AddressElement>));
            services.AddScoped(typeof(IGenericRepository<AddressElementType>), typeof(GenericRepository<AddressElementType>));
            services.AddScoped(typeof(IGenericRepository<Track>), typeof(GenericRepository<Track>));
            services.AddScoped(typeof(IGenericRepository<User>), typeof(GenericRepository<User>));

            // add services
            services.AddScoped<ArtistService>();
            services.AddScoped<TrackService>();
            services.AddScoped<UserService>();
            services.AddScoped<AddressService>();
            services.AddScoped<AuthService>();
        }
    }
}
