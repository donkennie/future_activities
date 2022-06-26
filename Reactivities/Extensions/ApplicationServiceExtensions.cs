using MediatR;
using Microsoft.EntityFrameworkCore;
using Reactivities.Infrastructure.Photos;
using Reactivities.Infrastructure.Security;
using Reactivities.Persistence;
using Reactivties.Application.Activities;
using Reactivties.Application.Core;
using Reactivties.Application.Interfaces;

namespace Reactivities.API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("DefaultConnection");

            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlServer(connectionString);
            });


            services.AddMediatR(typeof(List.Handler).Assembly);

            services.AddAutoMapper(typeof(MappingProfiles).Assembly);

            services.AddScoped<IPhotoAccessor, PhotoAccessor>();

            services.AddScoped<IUserAccessor, UserAccessor>();

            services.Configure<CloudinarySettings>(config.GetSection("Cloudinary"));

            services.AddSignalR();

            return services;    
        }
    }
}


/*var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<DataContext>(opt =>
{
    opt.UseSqlServer(connectionString);
});*/