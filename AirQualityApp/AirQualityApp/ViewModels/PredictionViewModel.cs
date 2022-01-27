using AirQualityApp.Models;
using AirQualityApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace AirQualityApp.ViewModels
{
    public class PredictionScreenViewModel : BaseViewModel
    {
        public ObservableCollection<String> Cities { get; }
        public ObservableCollection<String> PeriodOptions { get; }
        public ObservableCollection<ForecastData> PredictedValues { get; }
        public Command PerformForecastCommand { get; }
        public Command LoadCitiesCommand { get; }
        bool isBusyForecast = false;
        private Dictionary<String,Position> CityLocations { get; set; }
        public bool IsBusyForecast
        {
            get { return isBusyForecast; }
            set { SetProperty(ref isBusyForecast, value); }
        }

        string selectedCity = "Warsaw";
        public string SelectedCity
        {
            get { return selectedCity; }
            set { SetProperty(ref selectedCity, value); }
        }

        string forecastType = "7-day Forecast";
        public string ForecastType
        {
            get { return forecastType; }
            set { SetProperty(ref forecastType, value); }
        }

        public PredictionScreenViewModel()
        {

            Title = "Forecast";
            Cities = new ObservableCollection<String>();
            PeriodOptions = new ObservableCollection<String>();
            PredictedValues = new ObservableCollection<ForecastData>();

            PerformForecastCommand = new Command(async () => await PerformForecastMethod());
            LoadCitiesCommand = new Command(async () => await LoadCitiesMethod());

        }

        async Task LoadCitiesMethod()
        {
            
            IsBusy = true;

            try
            {
                Cities.Clear();
                Cities.Add("Athens");
                Cities.Add("Berlin");
                Cities.Add("Copenhagen");
                Cities.Add("Cracow");
                Cities.Add("Danzig");
                Cities.Add("London");
                Cities.Add("Madrid");
                Cities.Add("Milan");
                Cities.Add("Paris");
                Cities.Add("Prague");
                Cities.Add("Rome");
                Cities.Add("Turku");
                Cities.Add("Vienna");
                Cities.Add("Warsaw");
                CityLocations = new Dictionary<string, Position>();
                PeriodOptions.Clear();
                PeriodOptions.Add("7-day Forecast");
                PeriodOptions.Add("14-day Forecast");

                CityLocations.Add("Athens", new Position(37.98376, 23.72784));
                CityLocations.Add("Berlin", new Position(52.520008, 13.404954));
                CityLocations.Add("Copenhagen", new Position(55.676098, 12.568337));
                CityLocations.Add("Cracow", new Position(50.049683, 19.944544));
                CityLocations.Add("Danzig", new Position(54.372158, 18.638306));
                CityLocations.Add("London", new Position(51.509865, -0.118092));
                CityLocations.Add("Madrid", new Position(40.416775, -3.703790));
                CityLocations.Add("Milan", new Position(45.464664, 9.188540));
                CityLocations.Add("Paris", new Position(48.864716, 2.349014));
                CityLocations.Add("Prague", new Position(50.073658, 14.418540));
                CityLocations.Add("Rome", new Position(41.902782, 12.496366));
                CityLocations.Add("Turku", new Position(60.454510, 22.264824));
                CityLocations.Add("Vienna", new Position(48.210033, 16.363449));
                CityLocations.Add("Warsaw", new Position(52.237049, 21.017532));

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task PerformForecastMethod()
        {
            IsBusyForecast = true;
            if (string.IsNullOrWhiteSpace(SelectedCity))
            {
                await App.Current.MainPage.DisplayAlert("AirQualityApp", "Please select city.", "Ok");
                IsBusyForecast = false;
                return;
            }

            if (string.IsNullOrWhiteSpace(ForecastType))
            {
                await App.Current.MainPage.DisplayAlert("AirQualityApp", "Please select forecast type.", "Ok");
                IsBusyForecast = false;
                return;
            }
            try
            {
                PredictedValues.Clear();
                var x1 = new WeatherService();

                var items = await x1.RefreshDataAsync(CityLocations[SelectedCity]);
                
                var x2 = new RestService();
                
                var items2 = await x2.RefreshDataAsync(new BoundingBox(CityLocations[SelectedCity], 20));
                var item = items2.OrderBy(y => y.DistanceToUser).FirstOrDefault();
                if(item != null)
                {
                    int aqi = item.Aqi;
                    var z = App.ForecastProvider.GetForecast(items, aqi, SelectedCity, ForecastType);
                    PredictedValues.Clear();
                    foreach (var zz in z)
                    {
                        PredictedValues.Add(zz);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusyForecast = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }
    }
}
