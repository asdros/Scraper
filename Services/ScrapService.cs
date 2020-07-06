using Microsoft.Extensions.Configuration;
using Scraper.Services.Queries;
using System.Collections.Generic;
using System.Linq;
using Scraper.Services.ExecuteCommands;
using Scraper.Models;
using Dapper;

namespace Scraper.Services
{
    public class ScrapService:IScrapService
    {
        private readonly IConfiguration _configuration;
        private readonly ICommandText _commandText;
        private readonly string _connectionString;
        private readonly IExecuters _executers;

        public ScrapService(IConfiguration configuration,ICommandText commandText, IExecuters executers)
        {
            _commandText = commandText;
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
            _executers = executers;
        }

        public List<ScrapItem> GetItems()
        {
            return _executers.ExecuteCommand(_connectionString,
                connection => connection.Query<ScrapItem>(_commandText.GetItems)).ToList();
        }

        public void DropRows()
        {
            _executers.ExecuteCommand(_connectionString, connection =>
                connection.Query(_commandText.DropRows));
        }

        public void AddItem(ScrapItem item)
        {
            _executers.ExecuteCommand(_connectionString, connection =>
             {
                connection.Query<ScrapItem>(_commandText.AddItem,item);
             });
        }

        public  List<ScrapItem> GetItemsByHigherTemp(int temp)
        {
            return _executers.ExecuteCommand(_connectionString, connection => 
                connection.Query<ScrapItem>(_commandText.GetItemsByHigherTemp, new { @TempValue = temp })).ToList();
        }

        public List<ScrapItem> GetItemsByDay(byte dayNumber)
        {
            return _executers.ExecuteCommand(_connectionString, connection =>
                connection.Query<ScrapItem>(_commandText.GetItemsByDay, new { @DayNumber = dayNumber })).ToList();
        }

    }

}