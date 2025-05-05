using System.ComponentModel;
using ModelContextProtocol.Server;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMcpServer()
    .WithHttpTransport()
    .WithTools<WeatherForecastTool>();

var app = builder.Build();

app.MapMcp();

app.Run();

// 天気予報を取得するツール
[McpServerToolType, Description("天気予報を取得するツール")]
class WeatherForecastTool
{
    [McpServerTool, Description("指定した場所の天気予報を返します。")]
    public static string GetWeatherForecast(
        [Description("天気を取得したい場所の名前")]
        string location) => location switch
    {
        "東京" => "晴れ",
        "大阪" => "曇り",
        "札幌" => "雪",
        _ => "空からカエルが降る異常気象",
    };
}
