using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Dot.NetCoreRestApi.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

/*
 * 
 * Fetch Data from third party Api
 * 
 */
namespace Dot.NetCoreRestApi.Controllers
{
    [Route("api/[controller]")]
    public class LiveWeatherController : Controller
    {
        [HttpGet("[action]/{city}")]
        public IActionResult Index(string city)
        {
            return Ok(new { Temp = "12", Summary = "Barmy", City = city });
        }


        //AppId required for this
        [HttpGet("[action]/{city}")]
        public async Task<IActionResult> City(string city)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri("http://api.openweathermap.org");
                    var response = await client.GetAsync($"/data/2.5/weather?q={city}&appid=YOUR_API_KEY_HERE&units=metric");
                    response.EnsureSuccessStatusCode();

                    var stringResult = await response.Content.ReadAsStringAsync();
                    var rawWeather = JsonConvert.DeserializeObject<OpenWeatherResponse>(stringResult);
                    return Ok(new
                    {
                        Temp = rawWeather.Main.Temp,
                        Summary = string.Join(",", rawWeather.Weather.Select(x => x.Main)),
                        City = rawWeather.Name
                    });
                }
                catch (HttpRequestException httpRequestException)
                {
                    return BadRequest($"Error getting weather from OpenWeather: {httpRequestException.Message}");
                }

            }
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> JsonDataAsync()
        {

            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
                    var response = await client.GetAsync($"posts");
                    response.EnsureSuccessStatusCode();

                    var stringResult = await response.Content.ReadAsStringAsync();
                    //var postData = JsonConvert.DeserializeObject<PostsJson>(stringResult);
                    return Ok(stringResult);
                }
                catch (HttpRequestException httpRequestException)
                {
                    return BadRequest($"Error getting weather from OpenWeather: {httpRequestException.Message}");
                }
            }
        }
    }
}
