using ExpressYourself.Types;

namespace ExpressYourself.Exceptions
{
    public class HttpException : ExceptionBaseType
    {
        public HttpException(string message)
        {
            Code = "HTTP";
            CustomMessage = message;
            Severity = SeverityCodes.Non_user_error;
        }
    }
}
