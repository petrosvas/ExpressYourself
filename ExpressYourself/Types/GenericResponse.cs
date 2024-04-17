namespace ExpressYourself.Types
{
    public class GenericResponse<T>
    {
        public bool Found { get; set; }
        public T Value { get; set; }
    }
}
