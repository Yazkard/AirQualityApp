using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AirQualityApp.Models
{
    public class ForecastData
    {
        public DateTime Date { get; set; }
        public int Aqi { get; set; }
        public Color Color { get; set; }

        public ForecastData(DateTime datetime, int aqi)
        {
            if (aqi > 500)
            {
                Aqi = 1000 - aqi;
            } 
            else if(aqi < 0)
            {
                Aqi = -aqi;
            } else
            {
                Aqi = aqi;
            }
            Date = datetime;
            if (Aqi < 51)
            {
                Color = Color.Green;
            }
            else if (Aqi < 101)
            {
                Color = Color.Yellow;
            }
            else if (Aqi < 151)
            {
                Color = Color.Orange;
            }
            else if (Aqi < 201)
            {
                Color = Color.Red;
            }
            else if (Aqi < 301)
            {
                Color = Color.Purple;
            }
            else
            {
                Color = Color.Maroon;
            }
        }
    }
}
