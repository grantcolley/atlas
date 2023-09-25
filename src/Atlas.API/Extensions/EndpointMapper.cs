namespace Atlas.API.Extensions
{
    public static class EndpointMapper
    {
        public static WebApplication? MapEndpoints(this WebApplication app)
        {
            app.MapHealthChecks("/health");

            app.MapGet("/error", () => Results.Problem());

            app.MapAtlas();

            app.MapAtlasAdministration();

            return app;
        }
    }
}
