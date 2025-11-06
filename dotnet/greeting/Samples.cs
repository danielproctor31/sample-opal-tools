// Tool parameter models
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Optimizely.Opal.Tools;

namespace OpalToolsSDK.Sample;

public class GreetingParameters
{
    [Required]
    [Description("Name of the person to greet")]
    public string Name { get; set; } = string.Empty;

    [Description("Language for greeting (defaults to random)")]
    public string? Language { get; set; }
}

public class DateParameters
{
    [Description("Date format (defaults to ISO format)")]
    public string Format { get; set; } = "yyyy-MM-dd";
}

[Display(
    Name = "dans_greeting", 
    Description = "Greets a person in a random language (English, Spanish, or French)")]
public class GreetingTool(ILogger<GreetingTool> logger)
{
    [OpalTool(name: "Dan's .NET Greeting")]
    public Task<object> ExecuteAsync(GreetingParameters parameters, OpalToolContext context)
    {
        // Get parameters
        var name = parameters.Name;
        var language = parameters.Language;

        logger.LogInformation("Greeting tool called with name: {Name}, language: {Language}", name, language);

        // If language not specified, choose randomly
        if (string.IsNullOrEmpty(language))
        {
            var random = new Random();
            var languages = new[] { "english", "spanish", "french" };
            language = languages[random.Next(languages.Length)];
            logger.LogInformation("Language not specified, using random language: {Language}", language);
        }

        // Generate greeting based on language
        string greeting;
        if (language.ToLower() == "spanish")
        {
            greeting = $"¡Hola, {name}! ¿Cómo estás?";
        }
        else if (language.ToLower() == "french")
        {
            greeting = $"Bonjour, {name}! Comment ça va?";
        }
        else // Default to English
        {
            greeting = $"Hello, {name}! How are you?";
        }

        logger.LogInformation("Returning greeting: {Greeting}", greeting);

        return Task.FromResult<object>(new
        {
            greeting,
            language
        });
    }
}
//
// [Display(
//     Name = "todays-date", 
//     Description = "Returns today's date in the specified format"
// )]
// public class TodaysDateTool(ILogger<TodaysDateTool> logger)
// {
//     [OpalTool(name: "Todays Date")]
//     public Task<object> ExecuteAsync(DateParameters parameters, OpalToolContext context)
//     {
//         // Get parameters
//         var format = parameters.Format;
//
//         logger.LogInformation("Today's date tool called with format: {Format}", format);
//
//         // Get today's date
//         var today = DateTime.Now;
//
//         // Format the date
//         var formattedDate = today.ToString(format);
//
//         logger.LogInformation("Returning formatted date: {Date}", formattedDate);
//
//         return Task.FromResult<object>(new
//         {
//             date = formattedDate,
//             format,
//             timestamp = ((DateTimeOffset)today).ToUnixTimeSeconds()
//         });
//     }
// }
// [Display(
//     Name = "auth-example", 
//     Description ="Example of a tool that requires authentication")
// ]
// public class AuthExampleTool(ILogger<AuthExampleTool> logger)
// {
//     [OpalAuthorization("google", "calendar")]
//     [OpalTool(name: "Auth Example")]
//     public Task<object> ExecuteAsync(GreetingParameters parameters, OpalToolContext context)
//     {
//         logger.LogInformation("Auth example tool called with name: {Name}", parameters.Name);
//         logger.LogInformation("Auth provider: {Provider}", context.AuthorizationData?.Provider ?? "none");
//
//         return Task.FromResult<object>(new
//         {
//             message = $"Hello, {parameters.Name}! You are authenticated with {context.AuthorizationData?.Provider ?? "no provider"}.",
//             authProvided = context.AuthorizationData != null
//         });
//     }
// }