namespace ExpressYourself.Types
{
    public class Response<T> : HttpResponseMessage
    {
        public T? ResponseObject { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string Severity { get; set; }
        public override string ToString()
        {
            return $"HTTP Status Code: {StatusCode}.\n" +
                $"Response Object: {ResponseObject}.\n" +
                $"Error Code: {ErrorCode}.\n" +
                $"Error Message: {ErrorMessage}.\n" +
                $"Severity: {Severity}.\n";
        }
    }
}
