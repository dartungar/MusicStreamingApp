using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Repository;
using Service;

namespace WebApp
{
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// Add services for working with entities
        /// </summary>
        /// <param name="services"></param>
        public static void AddEntityServices(this IServiceCollection services)
        {
            services.AddTransient<UnitOfWork>();
            services.AddTransient<ArtistService>();
        }
    }
}
