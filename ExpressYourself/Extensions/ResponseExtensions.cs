using ExpressYourself.Exceptions;
using ExpressYourself.Types;

namespace ExpressYourself.Extensions
{
    public static partial class Extensions
    {
        public static Response<T> GetResponse<T>(this GenericResponse<T> genericResponse)
        {
            return new Response<T>
            {
                ResponseObject = genericResponse.Value,
                ErrorCode = "",
                ErrorMessage = ""
            };
        }

        public static Response<T> GetResponse<T>(this T genericResponse)
        {
            return new Response<T>
            {
                ResponseObject = genericResponse,
                ErrorCode = "",
                ErrorMessage = ""
            };
        }

        public static Response<TResult> CatchException<TResult>(this Exception ex)
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
