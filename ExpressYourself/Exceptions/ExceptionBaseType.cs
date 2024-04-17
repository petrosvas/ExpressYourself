using ExpressYourself.Types;

namespace ExpressYourself.Exceptions
{
    public abstract class ExceptionBaseType : Exception
    {
        public string? Code { get; set; }
        public string? CustomMessage { get; set; }
        public SeverityCodes Severity { get; set; } = SeverityCodes.None;
    }
}
