namespace IdentifierMatching.Models;

/// <summary>
/// Represents a pair of matched records from the base and target datasets.
/// </summary>
public record MatchedPair
{
    /// <summary>
    /// Gets the index of the record in the base dataset.
    /// </summary>
    public int BaseIndex { get; set; }

    /// <summary>
    /// Gets the index of the record in the target dataset.
    /// </summary>
    public int TargetIndex { get; set; }
}
