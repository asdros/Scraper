namespace Scraper.Models
{
    public struct ScrapItem
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string Sunrise { get; set; }
        public string Sunset { get; set; }
        public string TempDay { get; set; }
        public string TempNight { get; set; }
        public string Pressure { get; set; }
        public string RainFall { get; set; }
        public string MoonPhase { get; set; }
        public string FishingQuality { get; set; }
        public string City { get; set; }

    }
}
