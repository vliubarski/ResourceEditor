using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
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
		public ActionResult<Product> SaveProducts([FromBody] Product product)
		{
			return new ActionResult<Product>(product);
		}

		[HttpGet("[action]/{lookFor}")]
		public IEnumerable<object> Products(string lookFor)
		{
			var resource = _resourceService.GetAnyResource(lookFor);

			//var ret = new List<object>()
			//{
			//	new 
			//	{
			//		//id = 1,
			//		productName = "Vittttttt",
			//		productCode = "Liub",
			//		description = "good",
			//		starRating0 = 5,
			//		starRating1 = 6,
			//		starRating2 = 7,
			//		starRating3 = 8,
			//		starRating = 9,
			//		resourceType = "qqq",
			//		resourceKey = "kkk",
			//	}
			//};

			return  resource;//ret;//.Where(x => x.description.Contains(lookFor));
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

		public class Product
		{
			public string resourceType { get; set; }
			public string resourceKey { get; set; }
			public string resourceValue { get; set; }
			public string cultureCode { get; set; }
		}

	}
}
