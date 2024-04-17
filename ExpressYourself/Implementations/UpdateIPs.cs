using ExpressYourself.Exceptions;
using ExpressYourself.Interfaces;
using ExpressYourself.Types;
using Microsoft.Extensions.Options;

namespace ExpressYourself.Implementations
{
    public class UpdateIPs : BackgroundService
    {
        private readonly TimeSpan _timespan = TimeSpan.FromHours(1);
        private ILogger<UpdateIPs> _logger;
        private readonly Settings _settings;
        private readonly IServiceProvider _serviceProvider;

        public UpdateIPs(ILogger<UpdateIPs> logger, IOptions<Settings> settings, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _settings = settings.Value;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var service = scope.ServiceProvider.GetRequiredService<IExpressYourselfService>();


                    _logger.LogInformation($"Starting Updating IPs in Background Task. Using Entity framework: {_settings.UseEntityFramework}");

                    try
                    {
                        var update = _settings.UseEntityFramework ? await service.UpdateIPsEntityFramework() : await service.UpdateIPs();
                        _logger.LogInformation(update.ToString());
                        _logger.LogInformation("Update IPs Background Task Completed!");
                    }
                    catch (Exception ex)
                    {
                        var response = CatchException<object?>(ex);
                        if (response.Severity != SeverityCodes.None.ToString() && response.Severity != SeverityCodes.Non_user_error.ToString())
                            _logger.LogError(response.ToString());
                    }
                }
                await Task.Delay(_timespan, cancellationToken);
            }
        }

        private static Response<TResult> CatchException<TResult>(Exception ex)
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
