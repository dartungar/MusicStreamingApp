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
            // TO DO: разумно ли создавать Unit Of Work как Singleton?
            // +: меньше запросов к БД, вроде бы меньше нагрузки
            // -: вроде как копит утечки памяти, может сильно разрастись
            services.AddSingleton<UnitOfWork>();

            // add services
            services.AddScoped<ArtistService>();
            services.AddScoped<TrackService>();
            services.AddScoped<UserService>();
            services.AddScoped<AddressService>();
            services.AddScoped<AuthService>();
        }
    }
}
