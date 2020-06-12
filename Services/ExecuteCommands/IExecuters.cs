
using System.Data.SqlClient;
using System;


namespace Scraper.Services.ExecuteCommands
{
    public interface IExecuters
    {
        public void ExecuteCommand(string connStr, Action<SqlConnection> task);
        public T ExecuteCommand<T>(string connStr, Func<SqlConnection, T> task);

    }
}