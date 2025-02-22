namespace IdentifierMatching.Services;

/// <summary>
/// Interface for matching attributes between two datasets.
/// </summary>
public interface IAttributesMatching
{
    /// <summary>
    /// Matches records between base run and target run based on column values.
    /// </summary>
    /// <param name="baseRun">The base run.</param>
    /// <param name="targetRun">The target run.</param>
    /// <param name="baseColumns">The base columns.</param>
    /// <param name="targetColumns">The target columns.</param>
    /// <returns>The result of the matching operation.</returns>
    Task<MatchResult> MatchRecords(
        List<Dictionary<string, object>> baseRun,
        List<Dictionary<string, object>> targetRun,
        List<string> baseColumns,
        List<string> targetColumns);
}
