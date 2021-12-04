using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Maps;

namespace AirQualityApp.Models
{
    public class CustomMap : Map
    {
        public List<CustomPin> CustomPins { get; set; }

        public CustomMap()
        {
            CustomPins = new List<CustomPin>();
        }
    }

    public class CustomPin : Pin
    {
        public StationData station { get; set; }
    }
}
