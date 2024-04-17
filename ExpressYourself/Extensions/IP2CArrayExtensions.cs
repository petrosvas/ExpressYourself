namespace ExpressYourself.Extensions
{
    public static partial class Extensions
    {
        public static string Code(this string[] IP2CArray)
        {
            return IP2CArray[0];
        }

        public static string TwoLetterCode(this string[] IP2CArray)
        {
            return IP2CArray[1];
        }

        public static string ThreeLetterCode(this string[] IP2CArray)
        {
            return IP2CArray[2];
        }

        public static string Name(this string[] IP2CArray)
        {
            return IP2CArray[3];
        }
    }
}
