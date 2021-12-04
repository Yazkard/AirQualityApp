using AirQualityApp.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace AirQualityApp.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}