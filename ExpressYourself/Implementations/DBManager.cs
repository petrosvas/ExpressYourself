using ExpressYourself.Exceptions;
using ExpressYourself.Extensions;
using ExpressYourself.Interfaces;
using ExpressYourself.Types;
using System.Data;
using System.Data.SqlClient;

namespace ExpressYourself.Implementations
{
    public class DBManager : IDBManager
    {
        private readonly SqlConnection _sqlConnection = new SqlConnection("Server=(LocalDb)\\Local;Initial Catalog=master;Integrated Security=SSPI;Trusted_Connection=yes;");

        public async Task<GenericResponse<Sql_Countries>> GetCountryInfoByIp(string IP)
        {
            await OpenConnection();
            GenericResponse<Sql_Countries> resp = await GetCountryByIP(IP);
            await CloseConnection();
            return resp;
        }

        public async Task<GenericResponse<Sql_Countries>> GetCountryInfoUsingArrayString(string IP, string[] IP2CArray)
        {
            await OpenConnection();
            Sql_Countries IP2CSql_Countries = IP2CArray.ToSql_Countries();

            GenericResponse<Sql_Countries> resp = await GetCountryByName(IP2CSql_Countries.Name);
            if (resp.Found)
            {
                await InsertIntoIpAddresses(resp.Value.Id.ToString(), IP);
                await CloseConnection();
                return new GenericResponse<Sql_Countries>
                {
                    Found = true,
                    Value = IP2CSql_Countries
                };
            }
            await AddCountryAndIpAddressInfoToDB(IP2CSql_Countries, IP);
            await CloseConnection();
            return new GenericResponse<Sql_Countries>
            {
                Found = true,
                Value = IP2CSql_Countries
            };
        }

        private async Task<GenericResponse<Sql_Countries>> GetCountryByIP(string IP)
        {
            var command = GetConnectionCommand(Queries.GetCountryByIP, new SqlParameter[]
           {
                new SqlParameter("@ip", SqlDbType.VarChar){Value = IP }
           });
            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    return new GenericResponse<Sql_Countries>
                    {
                        Found = true,
                        Value = reader.ToSql_Countries()
                    };
                }
                return new GenericResponse<Sql_Countries>
                {
                    Found = false,
                    Value = new Sql_Countries()
                };
            }
        }

        public async Task<GenericResponse<Sql_Countries>> GetCountryByName(string name)
        {
            var command = GetConnectionCommand(Queries.GetCountryByName, new SqlParameter[]
            {
                new SqlParameter("@name", SqlDbType.VarChar){Value = name }
            });
            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    if (!int.TryParse(reader["Id"].ToString(), out int id)) throw new DBException("Parsing Id from Countries Table failed!");
                    return new GenericResponse<Sql_Countries> { Found = true, Value = new Sql_Countries { Id = id } };
                }
                return new GenericResponse<Sql_Countries> { Found = false };
            }
        }

        private async Task InsertIntoIpAddresses(string countryId, string IP)
        {
            var rowsIPAddresses = ExecuteConnectorNonQuery(Queries.InsertIntoIpAddresses, new SqlParameter[]
                    {
                    new SqlParameter("@countryId", SqlDbType.Int){Value = countryId },
                    new SqlParameter("@ip", SqlDbType.VarChar){Value = IP }
                    });
            if (await rowsIPAddresses != 1) throw new DBException($"Insert into IP addresses query returned {rowsIPAddresses} affected rows!");
        }

        private async Task AddCountryAndIpAddressInfoToDB(Sql_Countries sql_Countries, string IP)
        {
            int newId = await AddCountryToDB(sql_Countries);

            await InsertIntoIpAddresses(newId.ToString(), IP);
        }

        public async Task<int> AddCountryToDB(Sql_Countries sql_Countries)
        {
            int maxId = await GetCountriesMaxID();

            var rowsCountries = ExecuteConnectorNonQuery(Queries.InsertIntoCountries, new SqlParameter[]
                    {
                    new SqlParameter("@id", SqlDbType.Int){Value = maxId + 1 },
                    new SqlParameter("@name", SqlDbType.VarChar){Value = sql_Countries.Name },
                    new SqlParameter("@twoLetterCode", SqlDbType.Char){Value = sql_Countries.TwoLetterCode },
                    new SqlParameter("@threeLetterCode", SqlDbType.Char){Value = sql_Countries.ThreeLetterCode }
                    });
            if (await rowsCountries != 1) throw new DBException($"Insert into countires query returned {rowsCountries} affected rows!");
            return maxId + 1;
        }

        private async Task<int> GetCountriesMaxID()
        {
            var command = GetConnectionCommand(Queries.GetCountriesMaxID, Array.Empty<SqlParameter>());
            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                if (reader.Read())
                {
                    if (!int.TryParse(reader["MAXID"].ToString(), out int maxId))
                    {
                        throw new DBException("Parsing Max Id from Countries Table failed!");
                    }
                    return maxId;
                }
                return 0;
            }
        }

        public async Task OpenConnection()
        {
            await _sqlConnection.OpenAsync();
        }

        public async Task CloseConnection()
        {
            await _sqlConnection.CloseAsync();
        }

        private SqlCommand GetConnectionCommand(string query, SqlParameter[] sqlParams)
        {
            SqlCommand command = new SqlCommand(query, _sqlConnection);
            command.Parameters.AddRange(sqlParams);
            return command;
        }

        private async Task<int> ExecuteConnectorNonQuery(string query, SqlParameter[] sqlParams)
        {
            SqlCommand command = new SqlCommand(query, _sqlConnection);
            command.Parameters.AddRange(sqlParams);
            return await command.ExecuteNonQueryAsync();
        }

        public async Task<GenericResponse<Sql_Report>> GetSqlTableReport()
        {
            await OpenConnection();
            GenericResponse<Sql_Report> resp = await GetSql_Report();
            await CloseConnection();
            return resp;
        }

        private async Task<GenericResponse<Sql_Report>> GetSql_Report()
        {
            Sql_Report sql_Report = new Sql_Report();
            var command = GetConnectionCommand(Queries.GetAddressesReport, Array.Empty<SqlParameter>());
            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    sql_Report.GetRow(reader);
                }
                return new GenericResponse<Sql_Report> { Found = true, Value = sql_Report };
            }
        }

        public async Task<GenericResponse<Sql_Update>> GetIPsForUpdate(int rowsOffset)
        {
            await OpenConnection();
            GenericResponse<Sql_Update> sql_Update = await GetSql_Update(rowsOffset);
            await CloseConnection();
            return sql_Update;
        }

        private async Task<GenericResponse<Sql_Update>> GetSql_Update(int rowsOffset)
        {
            Sql_Update sql_Update = new Sql_Update();
            var command = GetConnectionCommand(Queries.GetIPsToUpdate, new SqlParameter[]
            {
                new SqlParameter("@offset", SqlDbType.Int){Value = rowsOffset }
            });
            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                bool found = reader.HasRows;
                while (await reader.ReadAsync())
                {
                    sql_Update.GetRow(reader);
                }
                return new GenericResponse<Sql_Update> { Found = found, Value = sql_Update };
            }
        }

        public async Task ChangeCountryIdInIPAddresses(int id, string IP)
        {
            var rowsIPAddresses = ExecuteConnectorNonQuery(Queries.SetIPAdressCountryId, new SqlParameter[]
                    {
                    new SqlParameter("@countryId", SqlDbType.Int){Value = id },
                    new SqlParameter("@ip", SqlDbType.VarChar){Value = IP }
                    });
            if (await rowsIPAddresses != 1) throw new DBException($"Insert into IP Addresses query returned {rowsIPAddresses} affected rows!");
        }
    }
}
