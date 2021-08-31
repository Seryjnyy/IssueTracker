using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.Linq;

namespace IssueTrackerDataLibrary.DataAccess
{
    public static class SqlDataAccess
    {
        /// <summary>
        /// Uses the configuration manager to access the Web.config file
        /// and get the connection string for the desired database.
        /// </summary>
        /// <param name="connectionName">
        /// Name of the database.
        /// </param>
        /// <returns>
        /// Returns the respective connection string for the database.
        /// </returns>
        public static string GetConnectionString(string connectionName = "IssueTrackerDB")
        {
            return ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
        }

        /// <summary>
        /// A generic method to load in data from a database. It first uses Data and Data.SqlClient
        /// to connect to the database, then Dapper is used to query the database.
        /// </summary>
        /// <typeparam name="T">
        /// Represents the data we are querying for.
        /// </typeparam>
        /// <param name="sql">
        /// The sql statement used to query the database.
        /// </param>
        /// <returns>
        /// A list of data that was fetched from the database.
        /// </returns>
        public static List<T> LoadData<T>(string sql)
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                return cnn.Query<T>(sql).AsList();
            }
        }

        /// <summary>
        /// Method to load data in from the database. It allows for a paramitarised
        /// sql statement to be used.
        /// </summary>
        /// <typeparam name="T">
        /// Represents the data we are querying for.
        /// </typeparam>
        /// <typeparam name="G">
        /// Represents the model holding values to be used with the paramitarised sql
        /// statement.
        /// </typeparam>
        /// <param name="sql">
        /// The sql statement used to query the database.
        /// </param>
        /// <param name="parameters">
        /// Value for paramitarised sql statement.
        /// </param>
        /// <returns>
        /// A list of data that was fetched from the database.
        /// </returns>
        public static List<T> LoadData<T, G>(string sql, G parameters)
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                return cnn.Query<T>(sql, param : parameters).AsList();
            }
        }

        /// <summary>
        /// A generic method to save data into a database. It first uses Data and Data.SqlClient 
        /// to connect to the database, then Dapper is used to execute a sql statement along with
        /// the data being inserted.
        /// </summary>
        /// <param name="sql">
        /// The sql statement to be executed.
        /// </param>
        /// <param name="data">
        /// The data that will be inserted into the database.
        /// </param>
        /// <returns>
        /// The number of rows successfully inserted.
        /// </returns>
        public static int SaveData<T>(string sql, T data)
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                return cnn.Execute(sql, data);
            }
        }

        /// CANNOT GET IT TO WORK, PART OF 'GETTING ID OF INSERTED ROW'
/*        public static int GetScopeIdentity()
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                return cnn.Query<int>("select cast(@@IDENTITY as int)").FirstOrDefault();
            }
        }*/
    }
}
