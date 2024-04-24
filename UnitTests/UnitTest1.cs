using ExpressYourself.Entity_Framework.DBContext;
using ExpressYourself.Entity_Framework.Implementations;
using ExpressYourself.Entity_Framework.Interfaces;
using ExpressYourself.Entity_Framework.Types;
using ExpressYourself.Exceptions;
using ExpressYourself.Implementations;
using ExpressYourself.Interfaces;
using ExpressYourself.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework.Internal;
using System.Net;

namespace UnitTests
{
    public class Tests
    {
        private IExpressYourselfService _expressYourselfService;

        public Tests()
        {

        }

        [SetUp]
        public void Setup()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile($"appsettings.json", optional: false).Build();
            var services = new ServiceCollection();
            services.AddDbContext<AppDbContext>(Options => Options.UseSqlServer(WebApplication.CreateBuilder().Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IExpressYourselfService, ExpressYourselfService>();
            services.AddSingleton<ICachingManager, CachingManager>();
            services.Decorate<ICachingManager, CachingManagerDecorator>();
            services.AddScoped<IEFManager, EFManager>();
            services.AddScoped<IEFManagerAdapter, EFManagerAdapter>();
            services.AddSingleton<IHTTPManager, HTTPManager>();
            services.Decorate<IHTTPManager, HTTPManagerDecorator>();
            services.AddSingleton<IConfiguration>(configuration);
            services.AddLogging();
            var serviceProvider = services.BuildServiceProvider();
            _expressYourselfService = serviceProvider.GetRequiredService<IExpressYourselfService>();
        }

        [Test]
        public async Task GetIPDetailsEntityFramework()
        {
            Random rnd = new Random();
            for (int i = 0; i < 150; i++)
            {
                byte[] bytes = new byte[4];
                rnd.NextBytes(bytes);
                await TestEndpointGetIPDetailsEntityFramework(GetIP(bytes));
            }
            Assert.Pass();
        }

        private static string GetIP(byte[] bytes)
        {
            return $"{bytes[0]}.{bytes[1]}.{bytes[2]}.{bytes[3]}";
        }

        private async Task<Response<CountryInfo>> TestEndpointGetIPDetailsEntityFramework(string IP)
        {
            return await RequestHandlerAsync(async (input) => await _expressYourselfService.GetIpDetailsEntityFramework(IP), IP);
        }

        [Test]
        public async Task GetSqlReportEntityFramework()
        {
            await TestEndpointGetSqlReportEntityFramework();
            Assert.Pass();
        }

        private async Task<Response<List<SQLReport>>> TestEndpointGetSqlReportEntityFramework()
        {
            return await RequestHandlerAsync(_expressYourselfService.GetSqlReportEntityFramework);
        }

        [Test]
        public async Task UpdateIPsEntityFramework()
        {
            await TestEndpointUpdateIPsEntityFramework();
            Assert.Pass();
        }

        private async Task<Response<string>> TestEndpointUpdateIPsEntityFramework()
        {
            return await RequestHandlerAsync(_expressYourselfService.UpdateIPsEntityFramework);
        }

        private async Task<Response<TResult>> RequestHandlerAsync<T, TResult>(Func<T, Task<Response<TResult>>> func, T input)
        {
            try
            {
                Response<TResult> response = await func(input);
                response.StatusCode = HttpStatusCode.OK;
                return response;
            }
            catch (Exception ex)
            {
                var response = CatchException<TResult>(ex);
                if (response.Severity != SeverityCodes.None.ToString() && response.Severity != SeverityCodes.Non_user_error.ToString())
                {
                    Assert.Fail();
                }
                return response;
            }
        }

        private async Task<Response<TResult>> RequestHandlerAsync<TResult>(Func<Task<Response<TResult>>> func)
        {
            try
            {
                Response<TResult> response = await func();
                response.StatusCode = HttpStatusCode.OK;
                return response;
            }
            catch (Exception ex)
            {
                var response = CatchException<TResult>(ex);
                if (response.Severity != SeverityCodes.None.ToString() && response.Severity != SeverityCodes.Non_user_error.ToString())
                {
                    Assert.Fail();
                }
                return response;
            }
        }

        private Response<TResult> CatchException<TResult>(Exception ex)
        {
            if (ex is ExceptionBaseType)
            {
                return new Response<TResult>
                {
                    ErrorCode = ((ExceptionBaseType)ex).Code,
                    ErrorMessage = ((ExceptionBaseType)ex).CustomMessage,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    Severity = ((ExceptionBaseType)ex).Severity.ToString()
                };
            }
            return new Response<TResult>
            {
                ErrorCode = "0000",
                ErrorMessage = $"An uncaught exception was thrown.\nType: {ex.GetType()}.\nMessage: {ex.Message}.\nStack Trace: {ex.StackTrace}",
                StatusCode = System.Net.HttpStatusCode.InternalServerError,
                Severity = SeverityCodes.Catastrophic.ToString()
            };
        }
    }
}