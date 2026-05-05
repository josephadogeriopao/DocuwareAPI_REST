using Microsoft.AspNetCore.Mvc;
using OPAOWebService.Server.Business;
using OPAOWebService.Server.Business.Interfaces;
using OPAOWebService.Server.Data.Constants;
using OPAOWebService.Server.Data.Repositories.Interfaces;
using OPAOWebService.Server.Infrastructure.Helpers;
using OPAOWebService.Server.Models.DTOs.Requests;
using OPAOWebService.Server.Models.DTOs.Responses;
using OPAOWebService.Server.Models.Exceptions;

namespace OPAOWebService.Server.Controllers
{

    public class LogController : ApiControllerBase
    {
        private readonly ILogService _logService;

        public LogController(ILogService logService)
        {
            _logService = logService;
        }

        [HttpGet("LogEntries", Name = "GetLogEntries")]
        public async Task<IActionResult> GetLogs()
        {
            var logs = await _logService.GetApiLogsAsync();
            return Ok(logs); // Returns a valid JSON array []
        }
    }

}








