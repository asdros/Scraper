using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Scraper.Models;
using Scraper.Services;
using Microsoft.AspNetCore.Http;
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

		[HttpGet]
		public ActionResult GetAll()
		{
			var items = _scrapService.GetItems();
			return Ok(JsonConvert.SerializeObject(items, Formatting.Indented));
		}

		[HttpPost("{city}")]
		public ActionResult AddItem(string city)
		{
			var html = $@"https://www.ekologia.pl/pogoda/polska/{city}/dla-wedkarzy,15-dni";
			HtmlWeb web = new HtmlWeb();

			var htmlDoc = web.Load(html);

			// var path = @"C:\Users\Szymon\source\repos\ConsoleApp1\website.html";
			// var htmlDoc = new HtmlDocument();
			// htmlDoc.Load(path);

			int j = 1;
			string date, sunrise, sunset, tempDay, tempNight, pressure, rainFall, moonPhase, fishingQuality;
			date = sunrise = sunset = tempDay = tempNight = pressure = rainFall = moonPhase = fishingQuality = string.Empty;


			bool first = true;

			var elements = htmlDoc.DocumentNode.SelectNodes("//div[@class='day-info' or @class='dzien  odd' or @class='dzien  even']");
			foreach (var element in elements)
			{

				if (first)              //skipping the first one empty instance of 'day-info' element
				{
					first = false;
					continue;
				}

				if (j % 2 != 0)
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

					_scrapService.AddItem(new ScrapItem
					{
						Date = date,
						Sunrise = sunrise,
						Sunset = sunset,
						TempDay = tempDay,
						TempNight = tempNight,
						Pressure = pressure,
						RainFall = rainFall,
						MoonPhase = moonPhase,
						FishingQuality = fishingQuality,
						City = city
					});
				}
				j++;
			}
			
			return Ok("DONE");

		}

		[HttpDelete]
		public ActionResult ClearDB()
		{
			_scrapService.DropRows();
			return Ok("DONE");
		}

		[HttpGet("hightemp/{temp}")]
		public ActionResult GetItemsByHigherTemp(int temp)
		{
			var items = _scrapService.GetItemsByHigherTemp(temp);
			return Ok(JsonConvert.SerializeObject(items, Formatting.Indented));
		}
		[AllowAnonymous]
		[HttpGet("byday/{number}")]
		public ActionResult GetItemsByDay(byte number)
		{
			var items = _scrapService.GetItemsByDay(number);
			return Ok(JsonConvert.SerializeObject(items, Formatting.Indented));
		}
	}
}