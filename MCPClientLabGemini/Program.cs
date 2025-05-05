#pragma warning disable SKEXP0070

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.Google;
using ModelContextProtocol.SemanticKernel.Extensions;

var builder = Kernel.CreateBuilder();
builder.Services.AddLogging(c => c.AddDebug().SetMinimumLevel(LogLevel.Trace));

builder.Services.AddGoogleAIGeminiChatCompletion(
    serviceId: "gemini",
    modelId: "gemini-2.0-flash-lite",
    // modelId: "gemini-2.5-pro-exp-03-25",
    apiKey: Environment.GetEnvironmentVariable("GEMINI_API_KEY")!);

var kernel = builder.Build();

await kernel.Plugins.AddMcpFunctionsFromSseServerAsync("WeatherForecast", "http://localhost:5000/sse");

var executionSettings = new GeminiPromptExecutionSettings
{
    Temperature = 0,
    // FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(),
    ToolCallBehavior = GeminiToolCallBehavior.AutoInvokeKernelFunctions
};

var prompt = "今日の群馬の天気は？";
var result = await kernel.InvokePromptAsync(prompt, new(executionSettings)).ConfigureAwait(false);
Console.WriteLine($"\n\n{prompt}\n{result}");