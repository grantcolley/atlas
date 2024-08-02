namespace Atlas.API.Extensions
{
    public static class EndpointMapper
    {
        public static WebApplication? MapEndpoints(this WebApplication app)
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
