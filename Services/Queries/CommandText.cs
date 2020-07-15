namespace Scraper.Services.Queries
{
    public class CommandText
    {
        public string AddItem => "INSERT INTO ScrapItem([Date],[Sunrise],[Sunset],[TempDay],[TempNight],[Pressure],[RainFall],[MoonPhase],[FishingQuality],[City])" +
            " VALUES(@date,@sunrise,@sunset,@tempDay,@tempNight,@pressure,@rainFall,@moonPhase,@fishingQuality,@city)";
        public string GetAllItems => "SELECT * FROM ScrapItem";
        public string GetItemsByDay => "Select * FROM ScrapItem WHERE LEFT(ScrapItem.Date,2)=@DayNumber";
        public string GetItemsAboveTheLowerTempRange => "SELECT * FROM ScrapItem WHERE TempDay>@TempValue ORDER BY TempDay DESC";
        public string DropRows => "DELETE FROM ScrapItem;DBCC CHECKIDENT ('[ScrapItem]', RESEED, 0)"; //clear table and reset identity
            
    }
}


