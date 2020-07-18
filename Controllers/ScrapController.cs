using Scraper.Models;
using Scraper.Services;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;

namespace Scraper.Controllers
{

	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class ScrapController : ControllerBase
	{
		private readonly IScrapService _scrapService;
		private readonly ScraperLogicService _scraperLogicService;

		public ScrapController(IScrapService scrapService, ScraperLogicService scraperLogicService)
		{
			_scrapService = scrapService;
			_scraperLogicService = scraperLogicService;

		}
		/// <summary>
		/// Displaying all results from the database.
		/// </summary>
		[HttpGet]
		public ActionResult GetAll()
		{
			List<ScrapItem> items=new List<ScrapItem> { };
			try
			{
				 items = _scrapService.GetAllItems();
			}
			catch
			{
				return Content("Error connecting to database");
			}
			return Ok(JsonConvert.SerializeObject(items, Formatting.Indented));
		}

		/// <summary>
		/// Scraping data from an Internet resource and sending it to be saved in a database.
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
			_scrapService.AddItem(_scraperLogicService.ScrapWebContent(city));
			return Ok("DONE");
		}

		/// <summary>
		/// Delete all records from the table.
		/// </summary>
		[HttpDelete]
		public ActionResult ClearDataBase()
		{
			try
			{
				_scrapService.DropRows();
			}
			catch
			{
				return Content("Error connecting to database");
			}
			return Ok("DONE");
		}

		/// <summary>
		/// Filtering results taking into account the lower limit of the temperature range and descending sorting.
		/// </summary>
		[HttpGet("hightemp/{temp}")]
		public ActionResult GetItemsAboveTheLowerTempRange(string temp)
		{
			bool success = Int32.TryParse(temp, out int result);
			if (!success)
			{
				return StatusCode(422,$"The value \"{temp}\" is not correct!");
			}
			var items = _scrapService.GetItemsAboveTheLowerTempRange(result);
			return Ok(JsonConvert.SerializeObject(items, Formatting.Indented));

		}

		/// <summary>
		/// Filtering results by day number.
		/// </summary>
		[AllowAnonymous]
		[HttpGet("byday/{number}")]
		public ActionResult GetItemsByDay(string number)
		{
			bool success = Byte.TryParse(number, out byte result);
			if (!success)
			{
				return StatusCode(422, $"The value \"{number}\" is not correct!");
			}
			var items = _scrapService.GetItemsByDay(result);
			return Ok(JsonConvert.SerializeObject(items, Formatting.Indented));
		}
	}
}