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
		/// Wyświetlanie z bazy danych wszystkich wyników.
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
			_scrapService.AddItem(_scraperLogicService.ScrapWebContent(city));
			return Ok("DONE");
		}

		/// <summary>
		/// Usuwanie z tabeli wszystkich rekordów.
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
		/// Filtrowanie wyników uwzględniając dolną granicę zakresu temperatur i sortowanie malejące.
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
		/// Filtrowanie wyników uwzględniając numer dnia.
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