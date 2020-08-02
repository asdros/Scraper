using HtmlAgilityPack;
using Scraper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Scraper.Services
{
    public class ScraperLogicService
    {
        public List<ScrapItem> ScrapWebContent(string city)
        {
            string html = $@"https://www.ekologia.pl/pogoda/polska/{city}/dla-wedkarzy,15-dni";
            HtmlWeb web = new HtmlWeb();

            var htmlDocument = web.Load(html);

            int evenHTMLTag = 1;              //the data of one object to be scraped is in two different parent html tags. Supervises the loop and sends data to the database every second pass
            char[] toRemoveFromHtmlNodeString = { 'm', '°', 'C', 'h', 'P', 'a' };
            ScrapItem scrapItem = new ScrapItem { };
            List<ScrapItem> tempStore = new List<ScrapItem>();
            bool first = true;

            var elements = htmlDocument.DocumentNode.SelectNodes("//div[@class='day-info' or @class='dzien  odd' or @class='dzien  even']");

            foreach (var element in elements)
            {
                if (first)              //skipping the first one empty instance of 'day-info' element
                {
                    first = false;
                    continue;
                }

                if (evenHTMLTag % 2 != 0)
                {
                    scrapItem.Date = HttpUtility.HtmlDecode(element.SelectSingleNode(".//span[@class='today']/b").InnerText);
                    scrapItem.Sunrise = HttpUtility.HtmlDecode(element.SelectSingleNode(".//span[@class='sunrise sprite']/b").InnerText);
                    scrapItem.Sunset = HttpUtility.HtmlDecode(element.SelectSingleNode(".//span[@class='sunset sprite']/b").InnerText);
                }
                else
                {
                    scrapItem.TempDay = Byte.Parse(HttpUtility.HtmlDecode(element.SelectSingleNode(".//div[@class='t-dzien']").InnerText).Trim(toRemoveFromHtmlNodeString));
                    scrapItem.TempNight = Byte.Parse(HttpUtility.HtmlDecode(element.SelectSingleNode(".//div[@class='t-noc']").InnerText).Trim(toRemoveFromHtmlNodeString));
                    scrapItem.Pressure = Single.Parse(HttpUtility.HtmlDecode(element.SelectSingleNode(".//div[@class='cisnienie-atmosferyczne sprite']").InnerText).Trim(toRemoveFromHtmlNodeString).Replace('.', ',')); // replace the decimal separator to allow proper parsing
                    scrapItem.RainFall = Int16.Parse(HttpUtility.HtmlDecode(element.SelectSingleNode(".//div[@class='opady sprite']").InnerText).Trim(toRemoveFromHtmlNodeString));
                    scrapItem.MoonPhase = HttpUtility.HtmlDecode(element.SelectSingleNode(".//div[@class='faza-ksiezyca sprite']/b").InnerText).Trim();
                    scrapItem.FishingQuality = HttpUtility.HtmlDecode(element.SelectSingleNode(".//div[@class='brania-opis']").InnerText);

                    scrapItem.City = char.ToUpper(city[0]) + city.Substring(1);

                    tempStore.Add(scrapItem);
                }
                evenHTMLTag++;
            }

            if (!tempStore.Any())
            {
                throw new InvalidOperationException("Not found the city name");
            }

            return tempStore;
        }
    }
}
