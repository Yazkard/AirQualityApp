using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace AirQualityApp.Models
{
    public class StationData
    {
        public Position Location { get; set; }
        public int Uid { get; set; }
        public int Aqi { get; set; }
        public string Name { get; set; }
        public DateTime Time { get; set; }
        public double DistanceToUser { get; set; }

        public Color Color { get; set; }

        public StationData()
        {

        }
        public StationData(RestResponse restResponse, Position UserLocation)
        {
            Location = new Position(restResponse.lat,restResponse.lon);
            DistanceToUser = Distance.BetweenPositions(Location, UserLocation).Kilometers;
            Uid = restResponse.uid;
            int x;
            bool v = Int32.TryParse(restResponse.aqi, out x);
            if (v)
            {
                Aqi = x;
                if (x < 51)
                {
                    Color = Color.Green;
                } else if (x < 101) {
                    Color = Color.Yellow;
                } else if (x < 151) {
                    Color = Color.Orange;
                } else if (x < 201) {
                    Color = Color.Red;
                } else if (x < 301) {
                    Color = Color.Purple;
                } else {
                    Color = Color.Maroon;
                }
            } else {
                Color = Color.White;
            }
            Name = restResponse.station.name;
            Time = DateTime.Parse(restResponse.station.time);

        }
    }

    public class MapRestResponse
    {
        public string status { get; set; }
        public List<RestResponse> data { get; set; }
    }
    public class RestResponse
    {
        public double lat { get; set; }
        public double lon { get; set; }
        public int uid { get; set; }
        public string aqi { get; set; }
        public StationDetails station { get; set; }
    }
    public class StationDetails
    {
        public string name { get; set; }
        public string time { get; set; }
    }
}
