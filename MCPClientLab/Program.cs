using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol.Transport;

var clientTransport = new SseClientTransport(new()
{
    Name = "天気予報",
    Endpoint = new Uri("http://localhost:5000/sse")
});

await using var client = await McpClientFactory.CreateAsync(clientTransport);

// ツールの一覧を取得
var tools = await client.ListToolsAsync();
foreach (var tool in tools)
{
    Console.WriteLine($"{tool.Name}: {tool.JsonSchema}");
}

// GetWeatherForecast ツールを取得
var getWeatherForecastTool = tools.FirstOrDefault(t => t.Name == "GetWeatherForecast")
    ?? throw new InvalidOperationException();

// GetWeatherForecast ツールを実行
var response = await client.CallToolAsync(getWeatherForecastTool.Name, new Dictionary<string, object?>
{
    // ["location"] = "東京",
    ["location"] = "群馬",
});

// レスポンスを表示
foreach (var content in response.Content)
{
    Console.WriteLine($"{content.Type}, {content.Text}");
}
