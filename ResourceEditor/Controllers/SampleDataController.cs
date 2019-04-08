using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace ResourceEditor.Controllers
{
	[Route("api/[controller]")]
	public class SampleDataController : Controller
	{
		private readonly IResourceService _resourceService;

		public SampleDataController(IResourceService resourceService)
		{
			_resourceService = resourceService;
		}

		private static string[] Summaries = new[]
		{
			"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
		};

		[HttpGet("[action]")]
		public IEnumerable<WeatherForecast> WeatherForecasts()
		{
			var rng = new Random();
			return Enumerable.Range(1, 5).Select(index => new WeatherForecast
			{
				DateFormatted = DateTime.Now.AddDays(index).ToString("d"),
				TemperatureC = rng.Next(-20, 55),
				Summary = Summaries[rng.Next(Summaries.Length)]
			});
		}

		[HttpPost("[action]")]
		public ActionResult<DbResource> CreateResource([FromBody] DbResource newResource)
		{
			var  ret = _resourceService.CreateResource(newResource);
			return new ActionResult<DbResource>(ret);
		}

		[HttpGet("[action]/{lookFor}")]
		public IEnumerable<object> Products(string lookFor)
		{
			return _resourceService.GetAnyResource(lookFor);
		}

		public class WeatherForecast
		{
			public string DateFormatted { get; set; }
			public int TemperatureC { get; set; }
			public string Summary { get; set; }

			public int TemperatureF
			{
				get
				{
					return 32 + (int)(TemperatureC / 0.5556);
				}
			}
		}
	}
}
