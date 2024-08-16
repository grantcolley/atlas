namespace Atlas.API.Extensions
{
    internal static class EndpointMapper
    {
        internal static WebApplication? MapEndpoints(this WebApplication app)
        {
            app.MapHealthChecks("/health");

            app.MapGet("/error", () => Results.Problem());

            app.MapAtlasEndpoints();

            app.MapAtlasApplicationEndpoints();

            app.MapAtlasAdministrationEndpoints();

            app.MapAtlasSupportEndpoints();

            app.MapAtlasModulesEndpoints();

            return app;
        }
    }
}
