using AirQualityApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AirQualityApp.Services
{
    public interface IForecastProvider
    {
        List<ForecastData> GetForecast(List<WeatherData> weatherDatas, int aqi, string city, string type);
    }
}
