using Microsoft.Extensions.Configuration;
using Scraper.Services.Queries;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using Scraper.Services.ExecuteCommands;
using Scraper.Models;
using Dapper;


namespace Scraper.Services
{
    public class ScrapService:IScrapService
    {
        private readonly IConfiguration _configuration;
        private readonly ICommandText _commandText;
        private readonly string _connStr;
        private readonly IExecuters _executers;

        public ScrapService(IConfiguration configuration,ICommandText commandText, IExecuters executers)
        {
            _commandText = commandText;
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("DefaultConnection");
            _executers = executers;
        }

        public List<ScrapItem> GetItems()
        {
            var query = _executers.ExecuteCommand(_connStr,
                conn => conn.Query<ScrapItem>(_commandText.GetItems)).ToList();
            return query;
        }

        public void DropRows()
        {
            _executers.ExecuteCommand(_connStr, conn =>
             {
                 var query = conn.Query(_commandText.DropRows);
             });
        }

        public void AddItem(ScrapItem item)
        {
            _executers.ExecuteCommand(_connStr, conn =>
             {
                 var query = conn.Query<ScrapItem>(_commandText.AddItem,
                     new { Date = item.Date, Sunrise = item.Sunrise, Sunset = item.Sunset, TempDay = item.TempDay, 
                         TempNight = item.TempNight, Pressure = item.Pressure, RainFall = item.RainFall, 
                         MoonPhase = item.MoonPhase, FishingQuality = item.FishingQuality,City=item.City });
 
             });
        }


    }

}