using GenericProcedure.DATA;
using GenericProcedure.DATA.Models;
using Lib.GenericProcedure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace GenericProcedure.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly MyDbContext _context;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, MyDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet]
        public async Task<IActionResult> GetProcedure()
        {
            var parameterList = new List<SqlParameter>()
            {
                new SqlParameter{ParameterName="@Id",Value = 3},
                new SqlParameter{ParameterName="@Name",Value = "Test"},
                
            };
            return Ok(await _context.Call<ResultProcedure>("sp_resultProcedure", parameterList));
        }
    }
}