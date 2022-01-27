using AirQualityApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;

namespace AirQualityApp.Services
{
    public class WeatherService : IWeatherService
    {
        HttpClient client;
        JsonSerializerOptions serializerOptions;

        public List<WeatherData> Items { get; private set; }

        public WeatherService()
        {
            client = new HttpClient();
            serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        public async Task<List<WeatherData>> RefreshDataAsync(Position position)
        {
            Items = new List<WeatherData>();
            //MapRestResponse mapRestResponse = new MapRestResponse();

            Uri uri = new Uri(Constants.WeatherUrl + "/v1/forecast?" + "latitude=" + position.Latitude.ToString() + "&longitude=" + position.Longitude.ToString() + "&hourly=temperature_2m,relativehumidity_2m,pressure_msl,windspeed_10m&past_days=1");
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    //Debug.WriteLine(content);
                    var y1 = content.Split('{');
                    var y2 = y1[3].Split('}');
                    string longest = y1.OrderByDescending(s => s.Length).First().Split('}').First();
                    longest = longest.Replace("temperature_2m", "Temperature");
                    longest = longest.Replace("pressure_msl", "Pressure");
                    longest = longest.Replace("windspeed_10m", "WindSpeed");
                    longest = longest.Replace("relativehumidity_2m", "Humidity");
                    longest = "{" + longest.Replace("time", "Time") + "}";
                    WeatherRestResponse z = JsonSerializer.Deserialize<WeatherRestResponse>(longest);

                    Items = z.CalculateAverages();
                    //List<string> videogames = JsonConvert.DeserializeObject<List<string>>(json);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            //foreach (var x in mapRestResponse.data)
            //{
            //    Items.Add(new StationData(x, bb.center));
            //}
            return Items;
        }
    }
}
