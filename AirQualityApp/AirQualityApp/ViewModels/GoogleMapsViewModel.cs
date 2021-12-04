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
            Title = "Browse";
            _Map = map;
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            
            //ExecuteLoadItemsCommand();
            //MapItemsSourceList.Connect().Bind(MapItemsObservableCollection).Subscribe();
            //GoogleMap.ItemsSource = MapItemsObservableCollection;
            //Polygon polygon = new Polygon
            //{
            //    StrokeWidth = 8,
            //    StrokeColor = Color.FromHex("#1BA1E2"),
            //    FillColor = Color.FromHex("#881BA1E2"),
            //    Geopath =
            //    {
            //        new Position(47.6368678, -122.137305),
            //        new Position(47.6368894, -122.134655),
            //        new Position(47.6359424, -122.134655),
            //        new Position(47.6359496, -122.1325521),
            //        new Position(47.6424124, -122.1325199),
            //        new Position(47.642463,  -122.1338932),
            //        new Position(47.6406414, -122.1344833),
            //        new Position(47.6384943, -122.1361248),
            //        new Position(47.6372943, -122.1376912)
            //    }
            //};

            // add the polygon to the map's MapElements collection
            //map.MapElements.Add(polygon);
            //Pin pin = new Pin
            //{
            //    Label = "Santa Cruz",
            //    Address = "The city with a boardwalk",
            //    Type = PinType.Place,
            //    Position = new Position(47.9628066, -122.0194722)
            //};
            //map.Pins.Add(pin);
            //_Map.CustomPins = new List<CustomPin> { pin2 };
            //_Map.Pins.Add(pin2);

            //this.WhenValueChanged(y => y.MapItemsObservableCollection).Subscribe(z => UpdateMapPins());
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