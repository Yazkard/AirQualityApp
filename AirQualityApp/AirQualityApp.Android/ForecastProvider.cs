using AirQualityApp.Models;
using AirQualityApp.Services;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.IO;
using Java.Nio;
using Java.Nio.Channels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace AirQualityApp.Droid
{
    public class ForecastProvider : IForecastProvider
    {
        public ForecastProvider()
        {

        }

        [Obsolete]
        private MappedByteBuffer LoadModelFile(string city, string type)
        {
            AssetManager assets = Forms.Context.Assets;
            AssetFileDescriptor fileDescriptor = assets.OpenFd(city + type + ".tflite");
            FileInputStream inputStream = new FileInputStream(fileDescriptor.FileDescriptor);
            FileChannel fileChannel = inputStream.Channel;
            long startOffset = fileDescriptor.StartOffset;
            long declaredLength = fileDescriptor.DeclaredLength;
            return fileChannel.Map(FileChannel.MapMode.ReadOnly, startOffset, declaredLength);
        }

        [Obsolete]
        public List<ForecastData> GetForecast(List<WeatherData> weatherDatas, int aqi, string city, string type) {

            List<ForecastData> PredictedValues = new List<ForecastData>();
            
            int next_aqi = aqi;
            for (int i = 0; i < 7; i++)
            {
                Xamarin.TensorFlow.Lite.Interpreter interpreter = new Xamarin.TensorFlow.Lite.Interpreter(LoadModelFile(city, "Short"));
                float[][] output = new float[1][];
                output[0] = new float[1];
                float[][][] input = null;
                input = new float[1][][];
                input[0] = new float[1][];
                CustomScaler s = new CustomScaler();
                input[0][0] = new float[9] { s.ScaleAqi(next_aqi), s.ScaleHumidity(weatherDatas[i].Humidity), s.ScalePressure(weatherDatas[i].Pressure), s.ScaleTemperature(weatherDatas[i].Temperature), s.ScaleWind(weatherDatas[i].WindSpeed), s.ScaleHumidity(weatherDatas[i + 1].Humidity), s.ScalePressure(weatherDatas[i + 1].Pressure), s.ScaleTemperature(weatherDatas[i + 1].Temperature), s.ScaleWind(weatherDatas[i+1].WindSpeed) };
                var inputs = Java.Nio.FloatBuffer.FromArray(input);
                var outputs = Java.Nio.FloatBuffer.FromArray(output);
                interpreter.Run(inputs, outputs);
                //outputs.
                var x = outputs.ToArray<float[]>();
                PredictedValues.Add(new ForecastData(DateTime.Today.AddDays(i+1), s.ScaleBackAqi(x[0][0])));
                next_aqi = PredictedValues.Last().Aqi;
                interpreter.Dispose();
            }

            if(type == "7-day Forecast")
            {
                return PredictedValues;
            }

            for (int i = 0; i < 7; i++)
            {
                Xamarin.TensorFlow.Lite.Interpreter interpreter = new Xamarin.TensorFlow.Lite.Interpreter(LoadModelFile(city, "Long"));
                float[][] output = new float[1][];
                output[0] = new float[1];
                float[][][] input = null;
                input = new float[1][][];
                input[0] = new float[1][];
                CustomScaler s = new CustomScaler();
                input[0][0] = new float[7] { s.ScaleAqi(PredictedValues[i + 6].Aqi), s.ScaleAqi(PredictedValues[i + 5].Aqi), s.ScaleAqi(PredictedValues[i + 4].Aqi), s.ScaleAqi(PredictedValues[i + 3].Aqi), s.ScaleAqi(PredictedValues[i + 2].Aqi), s.ScaleAqi(PredictedValues[i + 1].Aqi), s.ScaleAqi(PredictedValues[i].Aqi), };
                var inputs = Java.Nio.FloatBuffer.FromArray(input);
                var outputs = Java.Nio.FloatBuffer.FromArray(output);
                interpreter.Run(inputs, outputs);
                //outputs.
                var x = outputs.ToArray<float[]>();
                PredictedValues.Add(new ForecastData(DateTime.Today.AddDays(i + 1), s.ScaleBackAqi(x[0][0])));
                interpreter.Dispose();
            }

            return PredictedValues;
        }
    }
}