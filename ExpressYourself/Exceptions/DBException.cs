using ExpressYourself.Types;

namespace ExpressYourself.Exceptions
{
    public class DBException : ExceptionBaseType
    {
        public DBException(string message)
        {
            Code = "DTBS";
            CustomMessage = message;
            Severity = SeverityCodes.High;
        }
    }
}
