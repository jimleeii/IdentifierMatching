using EndpointDefinition;

namespace IdentifierMatching.EndpointDefinitions;

/// <summary>
/// Defines the endpoints for the identifier matching feature.
/// </summary>
public class IdentifierMatchingEndpointDefinition : IEndpointDefinition
{
    private ILogger<IdentifierMatchingEndpointDefinition>? Logger;

    /// <summary>
    /// Defines the endpoints.
    /// </summary>
    /// <param name="app">The app.</param>
    /// <param name="env">The env.</param>
    /// <remarks>
    /// Defines a single POST endpoint, <c>api/IdentifierMatching</c>, that executes the
    /// <see cref="IdentifierMatchingAsync"/> function to match attributes between the base run
    /// and target run and returns the matches in the response body.
    /// </remarks>
    public void DefineEndpoints(WebApplication app, IWebHostEnvironment env)
    {
        // Define endpoints
        app.MapPost("/api/IdentifierMatching", IdentifierMatchingAsync);

        // Define different endpoints based on environment
        if (env.IsDevelopment())
        {
            app.MapGet("/api/IdentifierMatching/debug", () => "Debug endpoint");
            // Log configuration
            Logger!.LogInformation("Configuration: {Config}", Config.GetConfig());
        }
    }

    /// <summary>
    /// Defines the services.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <remarks>
    /// Registers the <see cref="AttributesMatching"/> as a singleton instance of <see cref="IAttributesMatching"/>.
    /// </remarks>
    public void DefineServices(IServiceCollection services)
    {
        Logger = services.BuildServiceProvider().GetRequiredService<ILogger<IdentifierMatchingEndpointDefinition>>();

        services.AddSingleton<IAttributesMatching, AttributesMatching>();
    }

    /// <summary>
    /// Matches records between base run and target run based on column values.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="request">The request.</param>
    /// <returns>IResult.</returns>
    private static async Task<IResult> IdentifierMatchingAsync(IAttributesMatching service, IdentifierMatchRequest request)
    {
        var matches = await service.MatchRecords(request.BaseRun, request.TargetRun, request.BaseColumns, request.TargetColumns);
        return Results.Ok(matches);
    }
}