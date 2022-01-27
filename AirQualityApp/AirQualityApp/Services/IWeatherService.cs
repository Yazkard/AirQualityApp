using AirQualityApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;

namespace AirQualityApp.Services
{
    public interface IWeatherService
    {
        Task<List<WeatherData>> RefreshDataAsync(Position position);
    }
}
