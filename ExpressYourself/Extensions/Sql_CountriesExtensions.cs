using ExpressYourself.Types;
using System.Data.SqlClient;

namespace ExpressYourself.Extensions
{
    public static partial class Extensions
    {
        public static Sql_Countries ToSql_Countries(this SqlDataReader reader)
        {
            return new Sql_Countries
            {
                Name = reader["Name"].ToString() ?? "",
                ThreeLetterCode = reader["ThreeLetterCode"].ToString() ?? "",
                TwoLetterCode = reader["TwoLetterCode"].ToString() ?? ""
            };
        }

        public static Sql_Countries ToSql_Countries(this string[] array)
        {
            return new Sql_Countries
            {
                Name = array.Name(),
                ThreeLetterCode = array.ThreeLetterCode(),
                TwoLetterCode = array.TwoLetterCode()
            };
        }
    }
}
