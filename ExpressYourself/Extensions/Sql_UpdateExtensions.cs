using ExpressYourself.Types;
using System.Data.SqlClient;

namespace ExpressYourself.Extensions
{
    public static partial class Extensions
    {
        public static Sql_Update GetRow(this Sql_Update update, SqlDataReader reader)
        {
            update.Rows.Add(
                new Sql_UpdateData
                {
                    IP = reader["IP"].ToString(),
                    Name = reader["Name"].ToString(),
                    ThreeLetterCode = reader["ThreeLetterCode"].ToString(),
                    TwoLetterCode = reader["TwoLetterCode"].ToString()
                }
            );
            return update;
        }
    }
}
