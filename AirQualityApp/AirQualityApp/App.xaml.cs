using AirQualityApp.Services;
using AirQualityApp.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AirQualityApp
{
    public partial class App : Application
    {
        public static IForecastProvider ForecastProvider { get; private set; }
        public App(IForecastProvider forecastProvider)
        {
            InitializeComponent();
            ForecastProvider = forecastProvider;
            MainPage = new AppShell();
        }

        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
