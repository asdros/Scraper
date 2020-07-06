using Scraper.Models;
using Scraper.Services;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace Scraper.Controllers
{

	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class ScrapController : ControllerBase
	{
		private readonly IScrapService _scrapService;

		public ScrapController(IScrapService scrapService)
		{
			_scrapService = scrapService;
		}
		/// <summary>
		/// Wyświetlanie z bazy danych wszystkich wyników.
		/// </summary>
		[HttpGet]
		public ActionResult GetAll()
		{
			var items = _scrapService.GetItems();
			return Ok(JsonConvert.SerializeObject(items, Formatting.Indented));
		}

		/// <summary>
		/// Scrapowanie danych z zasobu internetowego i przekazywanie ich do zapisania w bazie danych.
		/// </summary>
		/// <remarks>
		/// Sample request:
		/// 
		///			POST 
		///         { 
		///         
		///             "Id":10,
		///             "Date":"1 lipca 2020",
		///             "Sunrise":"4:31",
		///             "Sunset":"21:22",
		///             "TempDay":"28°C",
		///             "TempNight":"17°C",
		///             "Pressure":"1007hPa",
		///             "RainFall":"16mm",
		///             "MoonPhase":"po I kwadrze",
		///             "FishingQuality":"doskonale",
		///             "City":"pila"
		///         
		///         }
		///</remarks>
		[HttpPost("{city}")]
		public ActionResult AddItem(string city)
		{
		
			var html = $@"https://www.ekologia.pl/pogoda/polska/{city}/dla-wedkarzy,15-dni";
			var web = new HtmlWeb();

			var htmlDocument = web.Load(html);

			var evenHTMLTag = 1;              //the data of one object to be scraped is in two different parent html tags. Supervises the loop and sends data to the database every second pass

			var date = string.Empty;
			var sunrise = string.Empty;
			var sunset = string.Empty;
			var tempDay = string.Empty;
			var tempNight = string.Empty;
			var pressure = string.Empty;
			var rainFall = string.Empty;
			var moonPhase = string.Empty;
			var fishingQuality = string.Empty;

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
					date = HttpUtility.HtmlDecode(element.SelectSingleNode(".//span[@class='today']/b").InnerText);
					sunrise = HttpUtility.HtmlDecode(element.SelectSingleNode(".//span[@class='sunrise sprite']/b").InnerText);
					sunset = HttpUtility.HtmlDecode(element.SelectSingleNode(".//span[@class='sunset sprite']/b").InnerText);
				}
				else
				{
					tempDay = HttpUtility.HtmlDecode(element.SelectSingleNode(".//div[@class='t-dzien']").InnerText);
					tempNight = HttpUtility.HtmlDecode(element.SelectSingleNode(".//div[@class='t-noc']").InnerText);
					pressure = HttpUtility.HtmlDecode(element.SelectSingleNode(".//div[@class='cisnienie-atmosferyczne sprite']").InnerText);
					rainFall = HttpUtility.HtmlDecode(element.SelectSingleNode(".//div[@class='opady sprite']").InnerText);
					moonPhase = HttpUtility.HtmlDecode(element.SelectSingleNode(".//div[@class='faza-ksiezyca sprite']/b").InnerText);
					fishingQuality = HttpUtility.HtmlDecode(element.SelectSingleNode(".//div[@class='brania-opis']").InnerText);

					_scrapService.AddItem(new ScrapItem{});
				}
				evenHTMLTag++;
			}
			return Ok("DONE");
		}

		/// <summary>
		/// Usuwanie z tabeli wszystkich rekordów.
		/// </summary>
		[HttpDelete]
		public ActionResult ClearDataBase()
		{
			_scrapService.DropRows();
			return Ok("DONE");
		}

		/// <summary>
		/// Filtrowanie wyników uwzględniając dolną granicę zakresu temperatur i sortowanie malejące.
		/// </summary>
		[HttpGet("hightemp/{temp}")]
		public ActionResult GetItemsByHigherTemp(int temp)
		{
			var items = _scrapService.GetItemsByHigherTemp(temp);
			return Ok(JsonConvert.SerializeObject(items, Formatting.Indented));
		}

		/// <summary>
		/// Filtrowanie wyników uwzględniając numer dnia.
		/// </summary>
		[AllowAnonymous]
		[HttpGet("byday/{number}")]
		public ActionResult GetItemsByDay(byte number)
		{
			var items = _scrapService.GetItemsByDay(number);
			return Ok(JsonConvert.SerializeObject(items, Formatting.Indented));
		}
	}
}