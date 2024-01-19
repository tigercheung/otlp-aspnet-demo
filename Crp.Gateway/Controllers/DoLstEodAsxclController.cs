using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace Crp.Gateway.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoLstEodAsxclController : ControllerBase
{
    //injet an instance of httpclient so we can make request to ohter web api
    //private readonly HttpClient _httpClient;
    private readonly IHttpClientFactory _httpClientFactory;


    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<DoLstEodAsxclController> _logger;

    public DoLstEodAsxclController(
        ILogger<DoLstEodAsxclController> logger, 
        IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet(Name = "DoLstEodAsxcl")]
    public async Task<string> Get()
    {
        var response = await _httpClientFactory.CreateClient().GetStringAsync("https://localhost:5101/api/downloaddata");

        return response;

        //return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //{
        //    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
        //    TemperatureC = Random.Shared.Next(-20, 55),
        //    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        //})
        //.ToArray();
    }
}
