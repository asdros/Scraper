namespace Scraper.Models
{
    public struct ScrapItem
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string Sunrise { get; set; }
        public string Sunset { get; set; }
        public byte TempDay { get; set; }
        public byte TempNight { get; set; }
        public float Pressure { get; set; }
        public short RainFall { get; set; }
        public string MoonPhase { get; set; }
        public string FishingQuality { get; set; }
        public string City { get; set; }

    }
}
