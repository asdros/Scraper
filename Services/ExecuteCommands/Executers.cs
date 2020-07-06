using System;
using System.Data.SqlClient;

namespace Scraper.Services.ExecuteCommands
{
    public class Executers :IExecuters
    {
        public void ExecuteCommand(string connectionString, Action<SqlConnection> task)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                task(connection);
            }
        }
        public T ExecuteCommand<T>(string connectionString, Func<SqlConnection, T> task)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return task(connection);
            }
        }
    }
}