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
    public static WeatherForecast GetWeatherForecast(
        [Description("天気を取得したい場所の名前")]
        string location) => location switch
    {
        "東京" => new(location, "晴れ"),
        "大阪" => new(location, "曇り"),
        "札幌" => new(location, "雪"),
        _ => new(location, "空からカエルが降る異常気象"),
    };
}

record WeatherForecast(
    [property: Description("場所")]
    string Location,
    [property: Description("天気予報")]
    string Forecast);