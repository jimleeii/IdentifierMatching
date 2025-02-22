using System.Text.Json;

namespace IdentifierMatching;

/// <summary>
/// Configuration class for defining data contracts.
/// </summary>
public static class Config
{
    /// <summary>
    /// Gets a JSON string of data contract representing the configuration.
    /// </summary>
    /// <returns>A JSON string of data contract.</returns>
    /// <remarks>
    /// The configuration is as follows:
    /// <list type="number">
    /// <item>Base Run: Alice, Bob, and Charlie with respective age: 30, 25, and 35.</item>
    /// <item>Target Run: Alice, David, and Charlie with respective age: 30, 40, and 35.</item>
    /// <item>Attribute Columns: name and age.</item>
    /// </list>
    /// </remarks>
    public static string GetConfig()
    {
        // Define Base Run
        var baseRun = new List<Dictionary<string, object>>
        {
            new() { { "id", 1 }, { "name", "Alice" }, { "age", 30 } },
            new() { { "id", 2 }, { "name", "Bob" }, { "age", 25 } },
            new() { { "id", 3 }, { "name", "Charlie" }, { "age", 35 } }
        };

        // Define Target Run
        var targetRun = new List<Dictionary<string, object>>
        {
            new() { { "id", 1 }, { "name", "Alice" }, { "age", 30 } },
            new() { { "id", 4 }, { "name", "David" }, { "age", 40 } },
            new() { { "id", 3 }, { "name", "Charlie" }, { "age", 35 } }
        };

        // Define Attribute Columns
        var attributeColumns = new List<string> { "name", "age" };

        // Structure the configuration
        var config = new IdentifierMatchRequest
        {
            BaseRun = baseRun,
            TargetRun = targetRun,
            BaseColumns = attributeColumns,
            TargetColumns = attributeColumns,
        };

        return JsonSerializer.Serialize(config, options: new JsonSerializerOptions { WriteIndented = true });
    }
}