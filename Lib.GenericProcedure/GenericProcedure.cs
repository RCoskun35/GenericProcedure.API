using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Lib.GenericProcedure
{
    public static class GenericProcedure
    {
        public async static Task<List<T>> Call<T>(this DbContext context, string procedureName, List<SqlParameter> parameters)
        {
            try
            {
                var responseParameter = new SqlParameter("@response", SqlDbType.NVarChar, -1);
                responseParameter.Direction = ParameterDirection.Output;
                parameters.Insert(0, responseParameter);

                using (var sqlCommand = context.Database.GetDbConnection().CreateCommand())
                {
                    sqlCommand.CommandText = procedureName;
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddRange(parameters.ToArray());

                    if (sqlCommand.Connection.State != ConnectionState.Open)
                    {
                        await sqlCommand.Connection.OpenAsync();
                    }
                    await sqlCommand.ExecuteNonQueryAsync();

                    var responseValue = responseParameter.Value;
                    var result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<T>>(responseValue.ToString());

                    if (responseValue != null && responseValue != DBNull.Value)
                    {
                        return result;
                    }

                    return new List<T>();
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}
