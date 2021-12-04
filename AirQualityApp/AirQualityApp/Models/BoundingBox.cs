using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Maps;

namespace AirQualityApp.Models
{
    public class BoundingBox
    {
        public double minLat { get; set; }
        public double minLon { get; set; }
        public double maxLat { get; set; }
        public double maxLon { get; set; }
        public Position center { get; set; }

        public BoundingBox(double minLat, double minLon, double maxLat, double maxLon)
        {
            this.minLat = minLat;
            this.minLon = minLon;
            this.maxLat = maxLat;
            this.maxLon = maxLon;
            this.center = new Position((minLat + maxLat) / 2, (minLon + maxLon) / 2);
        }

        public BoundingBox(Position position, double radius)
        {
            center = position;
            var latRadian = ConvertToRadians(position.Latitude);

            double degLatKm = 110.574235;
            double degLongKm = 110.572833 * Math.Cos(latRadian);
            double deltaLat = radius / degLatKm;
            double deltaLong = radius / degLongKm;

            minLat = position.Latitude - deltaLat;
            minLon = position.Longitude - deltaLong;
            maxLat = position.Latitude + deltaLat;
            maxLon = position.Longitude + deltaLong;
        }

        public static double ConvertToRadians(double angle)
        {
            return (Math.PI / 180) * angle;
        }
    }
}
