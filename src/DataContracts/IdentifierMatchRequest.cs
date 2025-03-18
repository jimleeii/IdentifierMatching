namespace IdentifierMatching.DataContracts;

/// <summary>
/// Represents a request to match records between a base run and a target run based on column values.
/// </summary>
/// <remarks>
/// The request is used by the <see cref="IAttributesMatching"/> service to execute the matching operation.
/// </remarks>
public struct IdentifierMatchRequest
{
    /// <summary>
    /// Gets or sets the list of records in the base run.
    /// </summary>
    public required List<Dictionary<string, object>> BaseRun { get; set; }

    /// <summary>
    /// Gets or sets the list of records in the target run.
    /// </summary>
    public required List<Dictionary<string, object>> TargetRun { get; set; }

    /// <summary>
    /// Gets or sets the list of attribute columns in the base run.
    /// </summary>
    public required List<string> BaseColumns { get; set; }

    /// <summary>
    /// Gets or sets the list of attribute columns in the target run.
    /// </summary>
    public required List<string> TargetColumns { get; set; }
}