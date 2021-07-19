using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Repository;
using Service;

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
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));

            // add services
            services.AddScoped<ArtistService>();
            services.AddScoped<TrackService>();
            services.AddScoped<UserService>();
            services.AddScoped<AddressService>();
            services.AddScoped<AuthService>();
        }
    }
}
