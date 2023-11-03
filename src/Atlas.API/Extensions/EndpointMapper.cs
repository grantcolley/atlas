namespace Atlas.API.Extensions
{
    public static class EndpointMapper
    {
        public static WebApplication? MapEndpoints(this WebApplication app)
        {
            app.MapHealthChecks("/health");

            app.MapGet("/error", () => Results.Problem());

            app.MapAtlasEndpoints();

            app.MapAtlasNavigationEndpoints();

            app.MapAtlasAdministrationEndpoints();

            return app;
        }
    }
}
