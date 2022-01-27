using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AirQualityApp.Models
{
    public class WeatherData
    {
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public double WindSpeed { get; set; }
        public double Pressure { get; set; }

        public WeatherData(double temperaure, double humidity, double windSpeed, double pressure)
        {
            Temperature = temperaure;
            Humidity = humidity;
            WindSpeed = windSpeed;
            Pressure = pressure;
        }
    }


    public class WeatherRestResponse
    {
        public List<double> Temperature { get; set; }
        public List<double> Pressure { get;set; }
        public List<double> WindSpeed { get; set; }
        public List<int> Humidity { get; set; }
        public List<string> Time { get; set; }

        public List<WeatherData> CalculateAverages()
        {
            var averages = new List<WeatherData>();
            for (int i = 0; i < (int)Temperature.Count/24; i++)
            {
                int e = i+1 * 8 - 1;
                int s = i * 8;
                averages.Add(new WeatherData(Temperature.Skip(s).Take(8).Average(), Humidity.Skip(s).Take(8).Average(), WindSpeed.Skip(s).Take(8).Average(), Pressure.Skip(s).Take(8).Average()));
            }
            
            return averages;
        }
    }
}
