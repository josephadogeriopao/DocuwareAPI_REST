using Microsoft.AspNetCore.Mvc;
using OPAOWebService.Server.Business.Interfaces;
using OPAOWebService.Server.Data.Repositories.Interfaces;
using OPAOWebService.Server.Models.DTOs.Requests;
using OPAOWebService.Server.Models.DTOs.Responses;

namespace OPAOWebService.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaxController : ControllerBase
    {
        private readonly ITaxService _taxService;

        public TaxController(ITaxService taxService)
        {
            _taxService = taxService;
        }


        private static readonly string[] Summaries =
        [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];

        [HttpGet(Name = "GetOpenRoll")]
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

        [HttpPost("AssessmentUpdateStatus", Name = "PostAssessmentUpdateStatus")]
        public AssessmentStatusResponse PostAssessmentUpdateStatus([FromBody] AssessmentStatusRequest request)
        {
            var value = _taxService.UpdatePropertyValuation(request);

            IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables() // This maps the 'ConnectionStrings__OracleDbConnection'
            .Build();
            string? connectionString = configuration.GetConnectionString("OracleDbConnection");


            return new AssessmentStatusResponse
            {
                StatusCode = 1, Message = "Success : " + value + connectionString
            };
        }

    }

}
