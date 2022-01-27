using System;
using System.Collections.Generic;
using System.Text;

namespace AirQualityApp.Models
{
    public class CustomScaler
    {
        public float ScaleTemperature(double In)
        {
            float max = 40;
            float min = -30;

            float Out = (float)((In - min) / (max - min));
            return Out;
        }
        public float ScalePressure(double In)
        {
            float max = 1100;
            float min = 920;

            float Out = (float)((In - min) / (max - min));
            return Out;
        }
        public float ScaleHumidity(double In)
        {
            float max = 100;
            float min = 0;

            float Out = (float)((In - min) / (max - min));
            return Out;
        }
        public float ScaleWind(double In)
        {
            float max = 100;
            float min = 0;

            float Out = (float)((In - min) / (max - min));
            return Out;
        }
        public float ScaleAqi(float In)
        {
            float max = 500;
            float min = 0;

            float Out = (In - min) / (max - min);
            return Out;
        }

        public int ScaleBackAqi(float In)
        {
            float max = 500;
            float min = 0;

            float Out = In * (max - min) + min;
            return (int)Out;
        }
    }
}
