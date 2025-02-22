namespace IdentifierMatching.EndpointDefinitions;

/// <summary>
/// Defines the endpoints for the identifier matching feature.
/// </summary>
public class IdentifierMatchingEndpointDefinition : IEndpointDefinition
{
    /// <summary>
    /// Defines the endpoints.
    /// </summary>
    /// <param name="app">The app.</param>
    /// <remarks>
    /// Defines a single POST endpoint, <c>api/IdentifierMatching</c>, that executes the
    /// <see cref="IdentifierMatchingAsync"/> function to match attributes between the base run
    /// and target run and returns the matches in the response body.
    /// </remarks>
    public void DefineEndpoints(WebApplication app)
    {
        app.MapPost("api/IdentifierMatching", IdentifierMatchingAsync);
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
        services.AddSingleton<IAttributesMatching, AttributesMatching>();
    }

    /// <summary>
    /// Matches records between base run and target run based on column values.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="request">The request.</param>
    /// <returns>IResult.</returns>
    private async Task<IResult> IdentifierMatchingAsync(IAttributesMatching service, IdentifierMatchRequest request)
    {
        var matches = await service.MatchRecords(request.BaseRun, request.TargetRun, request.BaseColumns, request.TargetColumns);
        return Results.Ok(matches);
    }
}
