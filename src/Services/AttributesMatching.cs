using System.Text.Json;

namespace IdentifierMatching.Services;

/// <summary>
/// Provides functionality to match attributes between two datasets.
/// </summary>
public class AttributesMatching : IAttributesMatching
{
    /// <summary>
    /// Matches records between base run and target run based on column values.
    /// </summary>
    /// <param name="baseRun">The base run containing attribute values.</param>
    /// <param name="targetRun">The target run containing attribute values.</param>
    /// <param name="baseColumns">The list of attribute column names in the base run.</param>
    /// <param name="targetColumns">The list of attribute column names in the target run.</param>
    /// <returns>
    /// A <see cref="MatchResult"/> containing the matched records, unmatched records in the base run
    /// and unmatched records in the target run.
    /// </returns>
    public Task<MatchResult> MatchRecords(
        List<Dictionary<string, object>> baseRun,
        List<Dictionary<string, object>> targetRun,
        List<string> baseColumns,
        List<string> targetColumns)
    {
        // Step 1: Validate inputs
        if (baseColumns.Count != targetColumns.Count)
        {
            throw new ArgumentException("Base Run and Target Run attribute columns must have the same length.");
        }

        // Step 2: Initialize results
        var matches = new List<MatchedPair>();
        var unmatchedBase = new List<int>();
        var unmatchedTarget = new HashSet<int>(Enumerable.Range(0, targetRun.Count));

        // Step 3: Perform matching
        for (int baseIdx = 0; baseIdx < baseRun.Count; baseIdx++)
        {
            bool matchFound = false;

            for (int targetIdx = 0; targetIdx < targetRun.Count; targetIdx++)
            {
                // Compare attribute values
                if (AreAttributesMatching(baseRun[baseIdx], targetRun[targetIdx], baseColumns, targetColumns))
                {
                    matches.Add(new MatchedPair() { BaseIndex = baseIdx, TargetIndex = targetIdx});
                    unmatchedTarget.Remove(targetIdx);
                    matchFound = true;
                    break; // Stop searching once a match is found
                }
            }

            if (!matchFound)
            {
                unmatchedBase.Add(baseIdx);
            }
        }

        // Step 4: Convert unmatchedTarget to a list
        var unmatchedTargetList = unmatchedTarget.ToList();

        // Step 5: Return results
        return Task.FromResult(new MatchResult
        {
            Matches = matches,
            UnmatchedBase = unmatchedBase,
            UnmatchedTarget = unmatchedTargetList
        });
    }

    /// <summary>
    /// Determines whether the specified base and target records have matching attributes
    /// based on the provided column mappings.
    /// </summary>
    /// <param name="baseRecord">The base record containing attribute values.</param>
    /// <param name="targetRecord">The target record containing attribute values.</param>
    /// <param name="baseColumns">The list of attribute column names in the base record.</param>
    /// <param name="targetColumns">The list of attribute column names in the target record.</param>
    /// <returns>
    /// <c>true</c> if all specified attributes in the base record match the corresponding
    /// attributes in the target record; otherwise, <c>false</c>.
    /// </returns>
    private static bool AreAttributesMatching(
        Dictionary<string, object> baseRecord,
        Dictionary<string, object> targetRecord,
        List<string> baseColumns,
        List<string> targetColumns)
    {
        for (int i = 0; i < baseColumns.Count; i++)
        {
            string baseColumn = baseColumns[i];
            string targetColumn = targetColumns[i];

            if (!baseRecord.ContainsKey(baseColumn) || !targetRecord.ContainsKey(targetColumn))
            {
                return false; // Missing column in one of the records
            }

            if (!ValuesMatch(baseRecord[baseColumn], targetRecord[targetColumn]))
            {
                return false; // Attribute values do not match
            }
        }

        return true; // All attributes match
    }

    /// <summary>
    /// Determines if two values are equal.
    /// </summary>
    /// <param name="baseValue">The base value to compare.</param>
    /// <param name="targetValue">The target value to compare.</param>
    /// <returns>
    /// <c>true</c> if the values are equal; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// This method handles different numeric types, string comparisons, and other object
    /// comparisons. It trims whitespace from strings before comparing and uses
    /// <see cref="StringComparison.OrdinalIgnoreCase"/> for string comparisons.
    /// </remarks>
    private static bool ValuesMatch(object baseValue, object targetValue)
    {
        if (baseValue == null && targetValue == null) return true;
        if (baseValue == null || targetValue == null) return false;

        try
        {
            // Convert values to strings for comparison
            var baseString = baseValue.ToString();
            var targetString = targetValue.ToString();

            // Handle different numeric types
            var baseElement = (JsonElement)baseValue;
            var targetElement = (JsonElement)targetValue;

            // Handle different numeric types
            if (IsNumericType(baseElement) && IsNumericType(targetElement))
            {
                return Convert.ToDecimal(baseString) == Convert.ToDecimal(targetString);
            }

            // Handle string comparisons
            if (IsStringType(baseElement) || IsStringType(targetElement))
            {
                return string.Equals(baseString?.Trim(), targetString?.Trim(), StringComparison.OrdinalIgnoreCase);
            }

            return Equals(baseValue, targetValue);
        }
        catch (InvalidCastException)
        {
            return false;
        }
    }

    /// <summary>
    /// Determines if the given <paramref name="element"/> is an instance of a numeric type.
    /// </summary>
    /// <param name="element">The element to check.</param>
    /// <returns>
    /// <c>true</c> if the given <paramref name="element"/> is an instance of a numeric type;
    /// otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// This method checks if the given <paramref name="element"/> is an instance of one of the
    /// following types: <see cref="int"/>, <see cref="long"/>, <see cref="decimal"/>,
    /// <see cref="double"/>, or <see cref="float"/>.
    /// </remarks>
    private static bool IsNumericType(JsonElement element) => element.ValueKind is JsonValueKind.Number;

    /// <summary>
    /// Determines if the given <paramref name="element"/> is an instance of a string type.
    /// </summary>
    /// <param name="element">The element to check.</param>
    /// <returns>
    /// <c>true</c> if the given <paramref name="element"/> is an instance of a string type;
    /// otherwise, <c>false</c>.
    /// </returns>
    private static bool IsStringType(JsonElement element) => element.ValueKind is JsonValueKind.String;
}