using ExpressYourself.Exceptions;
using ExpressYourself.Types;
using System.Data.SqlClient;

namespace ExpressYourself.Extensions
{
    public static partial class Extensions
    {
        public static void GetRow(this Sql_Report sql_Report, SqlDataReader reader)
        {
            if (!int.TryParse(reader["AddressesCount"].ToString(), out int addressesCount))
                throw new DBException("Parsing int AddressesCount from query failed!");
            if (!DateTime.TryParse(reader["LastAddressUpdated"].ToString(), out DateTime lastAddressUpdated))
                throw new DBException("Parsing DateTime LastAddressUpdated from query failed!");

            sql_Report.Rows.Add(new Sql_ReportData
            {
                AddressesCount = addressesCount,
                LastAddressUpdated = lastAddressUpdated,
                CountryName = reader["CountryName"].ToString() ?? ""
            });
        }
    }
}
