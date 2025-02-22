namespace IdentifierMatching.Models;

/// <summary>
/// The result of a matching operation.
/// </summary>
public class MatchResult
{
    /// <summary>
    /// The matched record indices, where each tuple contains the base record index and the target record index.
    /// </summary>
    public List<MatchedPair> Matches { get; set; } = [];

    /// <summary>
    /// The indices of the base records that did not have a match.
    /// </summary>
    public List<int> UnmatchedBase { get; set; } = [];

    /// <summary>
    /// The indices of the target records that did not have a match.
    /// </summary>
    public List<int> UnmatchedTarget { get; set; } = [];
}
