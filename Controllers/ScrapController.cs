using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Scraper.Models;
using Scraper.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Scraper.Controllers
{
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
            return Ok(items);
        }



    }
}