using Application.Interfaces;
using Application.Interfaces.Account;
using Application.Interfaces.Context;
using Application.Interfaces.Message;
using Application.Services.Account;
using Application.Services.Message;
using BlazorChatRoom.Server.Services;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace BlazorChatRoom.Server.Extensions;

public static class ServiceCollectionExtensions
{

    internal static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(settings =>
        {
            
        });


        return services;
    }

    internal static IServiceCollection AddCurrentUserService(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        return services;
    }

    internal static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration) => services.AddDbContext<DataBaseContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

    internal static IServiceCollection AddIdentity(this IServiceCollection service)
    {
        service.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<DataBaseContext>();

        service.AddIdentityServer()
            .AddApiAuthorization<User, DataBaseContext>();

        return service;
    }

    internal static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IDataBaseContext, DataBaseContext>();
        services.AddScoped<IMessageService, MessageService>();
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}