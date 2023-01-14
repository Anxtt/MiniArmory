﻿using Microsoft.EntityFrameworkCore;

using MiniArmory.Core.Services;
using MiniArmory.Core.Services.Contracts;

using MiniArmory.Data.Data;

namespace MiniArmory.Web.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAchievementService, AchievementService>();
            services.AddScoped<IClassService, ClassService>();
            services.AddScoped<ICharacterService, CharacterService>();
            services.AddScoped<IFactionService, FactionService>();
            services.AddScoped<IMountService, MountService>();
            services.AddScoped<IRaceService, RaceService>();
            services.AddScoped<IRealmService, RealmService>();
            services.AddScoped<ISpellService, SpellService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }

        public static IServiceCollection AddAppDbContext(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("DefaultConnection");

            services.AddDbContext<MiniArmoryDbContext>(options =>
                options.UseSqlServer(connectionString));

            return services;
        }
    }
}