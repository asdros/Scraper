using System.Data.SqlClient;
using System;

namespace Scraper.Services.ExecuteCommands
{
    public interface IExecuters
    {
        public void ExecuteCommand(string connectionString, Action<SqlConnection> task);
        public T ExecuteCommand<T>(string connectionString, Func<SqlConnection, T> task);
    }
}