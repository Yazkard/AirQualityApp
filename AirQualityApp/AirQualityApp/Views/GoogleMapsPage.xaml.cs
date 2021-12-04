using AirQualityApp.Models;
using AirQualityApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace AirQualityApp.Views
{
    public partial class GoogleMapsPage : ContentPage
    {
        GoogleMapsViewModel _viewModel;
        CustomMap _map;
        public GoogleMapsPage()
        {
            InitializeComponent();
            _map = new CustomMap()
            {
                IsShowingUser = true,
                MapType = MapType.Street
            };
            Content = _map;
            BindingContext = _viewModel = new GoogleMapsViewModel(_map);
            SetLocation();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

        async Task SetLocation()
        {
            var x = await Geolocation.GetLocationAsync();
            _map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(x.Latitude, x.Longitude), Distance.FromKilometers(20.0)));
            await _viewModel.ExecuteLoadItemsCommand();
        }
    }
}