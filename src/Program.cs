using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointDefinitions(typeof(IEndpointDefinition));
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals;
});

var app = builder.Build();
app.UseEndpointDefinitions();

app.MapGet("/", () => "Hello IdentifierMatching!");

Console.WriteLine(Config.GetConfig());

await app.RunAsync();