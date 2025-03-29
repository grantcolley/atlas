namespace Atlas.API.Extensions
{
    internal static class EndpointMapper
    {
        internal static WebApplication? MapEndpoints(this WebApplication app)
        {
            app.MapHealthChecks("/health");

            app.MapGet("/error", () => Results.Problem());

            app.MapAtlasEndpoints();

            if (app.Environment.IsDevelopment())
            {
                app.MapAtlasDatabaseEndpoints();
            }

            app.MapAtlasApplicationEndpoints();

            app.MapAtlasAdministrationEndpoints();

            app.MapAtlasSupportEndpoints();

            app.MapAtlasModulesEndpoints();

            return app;
        }
    }
}
