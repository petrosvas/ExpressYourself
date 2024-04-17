using ExpressYourself.Types;

namespace ExpressYourself.Exceptions
{
    public class InputException : ExceptionBaseType
    {
        public InputException(string message)
        {
            Code = "IMPT";
            CustomMessage = message;
            Severity = SeverityCodes.User_error;
        }
    }
}
