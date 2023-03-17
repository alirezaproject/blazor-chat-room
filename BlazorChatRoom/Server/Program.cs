using Application.Extensions;
using Application.Interfaces.Account;
using Application.Interfaces.Context;
using Application.Interfaces.Message;
using Application.Services.Account;
using Application.Services.Message;
using BlazorChatRoom.Server;
using BlazorChatRoom.Server.Extensions;
using Domain.Entities;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using Serilog;


var builder = WebApplication
    .CreateBuilder(args);

builder.Host.UseSerilog();

builder.Services
    .AddApplicationLayer()
    .AddApplicationServices()
    .AddInfrastructureLayer()
    .AddSwagger()
    .AddDatabase(builder.Configuration)
    .AddCurrentUserService()
    .AddIdentity();


builder.Services.AddSignalR();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddAuthentication()
    .AddIdentityServerJwt();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();



var app = builder.Build();


app.UseSerilogRequestLogging();
app.UseExceptionHandling(app.Environment);
app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();


app.MapRazorPages();
app.MapControllers();
app.ConfigureSwagger();
app.MapFallbackToFile("index.html");
app.MapHub<ChatHub>("/chatHub");
app.Run();
