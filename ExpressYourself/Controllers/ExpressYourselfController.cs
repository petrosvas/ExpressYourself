using ExpressYourself.Entity_Framework.DBContext;
using ExpressYourself.Entity_Framework.Types;
using ExpressYourself.Extensions;
using ExpressYourself.Interfaces;
using ExpressYourself.Types;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ExpressYourself.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExpressYourselfController : ControllerBase
    {
        private readonly IExpressYourselfService _expressYourselfService;
        private readonly ILogger<ExpressYourselfController> _logger;
        private readonly AppDbContext _dbContext;

        public ExpressYourselfController(IExpressYourselfService expressYourselfService, ILogger<ExpressYourselfController> logger, AppDbContext dbContext)
        {
            _expressYourselfService = expressYourselfService;
            _logger = logger;
            _dbContext = dbContext;
        }

        private async Task<Response<TResult>> RequestHandlerAsync<T, TResult>(Func<T, Task<Response<TResult>>> func, T input)
        {
            try
            {
                Response<TResult> response = await func(input);
                response.StatusCode = HttpStatusCode.OK;
                _logger.LogInformation("Task has ended successfully!");
                return response;
            }
            catch (Exception ex)
            {
                var response = ex.CatchException<TResult>();
                _logger.LogError($"An error has occured. {response}");
                return response;
            }
        }

        private async Task<Response<TResult>> RequestHandlerAsync<TResult>(Func<Task<Response<TResult>>> func)
        {
            try
            {
                Response<TResult> response = await func();
                response.StatusCode = HttpStatusCode.OK;
                _logger.LogInformation("Task has ended successfully!");
                return response;
            }
            catch (Exception ex)
            {
                var response = ex.CatchException<TResult>();
                _logger.LogError($"An error has occured. {response}");
                return response;
            }
        }

        [HttpGet("GetIPDetails")]
        public async Task<Response<CountryInfo>> GetIPDetails(string IP)
        {
            return await RequestHandlerAsync(async (input) => await _expressYourselfService.GetIPDetails(IP), IP);
        }

        [HttpGet("GetSqlReport")]
        public async Task<Response<Sql_Report>> GetSqlReport()
        {
            return await RequestHandlerAsync(_expressYourselfService.GetSqlReport);
        }

        [HttpGet("GetIPDetailsEntityFramework")]
        public async Task<Response<CountryInfo>> GetIPDetailsEntityFramework(string IP)
        {
            return await RequestHandlerAsync(async (input) => await _expressYourselfService.GetIpDetailsEntityFramework(IP), IP);
        }

        [HttpGet("GetSqlReportEntityFramework")]
        public async Task<Response<List<SQLReport>>> GetSqlReportEntityFramework()
        {
            return await RequestHandlerAsync(_expressYourselfService.GetSqlReportEntityFramework);
        }
    }
}