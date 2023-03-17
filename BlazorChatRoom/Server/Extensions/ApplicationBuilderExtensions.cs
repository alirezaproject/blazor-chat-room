namespace BlazorChatRoom.Server.Extensions;

public static class ApplicationBuilderExtensions
{
    internal static IApplicationBuilder UseExceptionHandling(
        this IApplicationBuilder app,
        IWebHostEnvironment env)
    {
        if (!env.IsDevelopment()) return app;


        app.UseDeveloperExceptionPage();
        app.UseWebAssemblyDebugging();



        return app;
    }

    internal static void ConfigureSwagger(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blazor API V1");
        });

    }

}