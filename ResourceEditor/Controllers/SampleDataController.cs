using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace ResourceEditor.Controllers
{
	[Route("api/[controller]")]
	public class SampleDataController : Controller
	{
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

		[HttpGet("[action]/{lookFor}")]
		public IEnumerable<Product> Products(string lookFor)
		{
			var ret = new List<Product>()
			{
				new Product
				{
					id = 1,
					productName = "Vit	",
					productCode = "Liub",
					description = "good",
					starRating = 5
				}
			};

			return ret.Where(x => x.description.Contains(lookFor));
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
			public int id { get; set; }
			public string productName { get; set; }
			public string productCode { get; set; }
			public string description { get; set; }
			public int starRating { get; set; }
		}

	}
}
