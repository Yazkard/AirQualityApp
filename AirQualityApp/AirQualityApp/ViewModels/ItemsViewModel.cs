using AirQualityApp.Models;
using AirQualityApp.Services;
using AirQualityApp.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace AirQualityApp.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        private StationData _selectedItem;

        public ObservableCollection<StationData> Items { get; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command<StationData> ItemTapped { get; }

        public ItemsViewModel()
        {
            Title = "Browse";
            Items = new ObservableCollection<StationData>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            ItemTapped = new Command<StationData>(OnItemSelected);

            AddItemCommand = new Command(OnAddItem);
            
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;
            
            try
            {
                Items.Clear();
                var x = new RestService();
                var location = await Geolocation.GetLocationAsync();
                Position position = new Position(location.Latitude,location.Longitude);
                var items = await x.RefreshDataAsync(new BoundingBox(position, 20));
                items = items.OrderBy(y => y.DistanceToUser).ToList();
                foreach (var item in items)
                {
                    Items.Add(item);
                }
                
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

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }

        public StationData SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        private async void OnAddItem(object obj)
        {
            //await Shell.Current.GoToAsync(nameof(NewItemPage));
        }

        async void OnItemSelected(StationData item)
        {
            //if (item == null)
            //    return;

            //// This will push the ItemDetailPage onto the navigation stack
            //await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.Id}");
        }
    }
}