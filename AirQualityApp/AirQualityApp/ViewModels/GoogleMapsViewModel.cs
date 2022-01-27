using AirQualityApp.Models;
using AirQualityApp.Services;
using DynamicData;
using DynamicData.Binding;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace AirQualityApp.ViewModels
{
    public class GoogleMapsViewModel : BaseViewModel
    {
        private CustomMap _Map;
        private SourceCache<CustomPin, string> MapItemsSourceList = new SourceCache<CustomPin, string>(x => x.Label);

        public IObservableCollection<CustomPin> MapItemsObservableCollection;

        public Command LoadItemsCommand { get; }
        public GoogleMapsViewModel(CustomMap map)
        {
            Title = "Map";
            _Map = map;
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            
        }

        public async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                var z = _Map.VisibleRegion;
                var x = new RestService();
                var items = await x.RefreshDataAsync(new BoundingBox(z.Center, z.Radius.Kilometers));
                items = items.OrderBy(y => y.DistanceToUser).ToList();
                foreach (var item in items)
                {
                    var c = new CustomPin()
                    {
                        Label = item.Name,
                        Position = item.Location,
                        Type = PinType.Place,
                        Address = item.Name,
                        station = item
                    };
                    MapItemsSourceList.AddOrUpdate(c);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                UpdateMapPins();
                IsBusy = false;
            }
        }

        private void UpdateMapPins()
        {
            _Map.Pins.Clear();
            _Map.CustomPins.Clear();
            _Map.CustomPins.AddRange(MapItemsSourceList.Items);
            _Map.Pins.AddRange(MapItemsSourceList.Items);
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }
    }
}