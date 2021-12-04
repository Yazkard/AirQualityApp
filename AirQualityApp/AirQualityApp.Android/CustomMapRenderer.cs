using System;
using System.Collections.Generic;
using AirQualityApp.Droid;
using AirQualityApp.Models;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace AirQualityApp.Droid
{
    public class CustomMapRenderer : MapRenderer, GoogleMap.IInfoWindowAdapter
    {
        List<CustomPin> customPins;

        public CustomMapRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                NativeMap.InfoWindowClick -= OnInfoWindowClick;
            }

            if (e.NewElement != null)
            {
                var formsMap = (CustomMap)e.NewElement;
                customPins = formsMap.CustomPins;
            }
        }

        protected override void OnMapReady(GoogleMap map)
        {
            base.OnMapReady(map);

            NativeMap.InfoWindowClick += OnInfoWindowClick;
            NativeMap.SetInfoWindowAdapter(this);
        }

        protected override MarkerOptions CreateMarker(Pin pin)
        {
            var marker = new MarkerOptions();
            marker.SetPosition(new LatLng(pin.Position.Latitude, pin.Position.Longitude));
            marker.SetTitle(pin.Label);
            //marker.SetSnippet(pin.Address);
           
            //Bitmap b = Bitmap.CreateBitmap( Android.Graphics.Color.Red, 100, 100, null);
            //Canvas canvas = new Canvas(b);
            //Paint paint = new Paint();
            //paint.Color = Android.Graphics.Color.White;
            //paint.TextSize = 12;
            //canvas.DrawText("30", 20, 20, paint);
            marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.icon_about));
            return marker;
        }

        void OnInfoWindowClick(object sender, GoogleMap.InfoWindowClickEventArgs e)
        {
            var customPin = GetCustomPin(e.Marker);
            if (customPin == null)
            {
                throw new Exception("Custom pin not found");
            }

            //if (!string.IsNullOrWhiteSpace(customPin.Url))
            //{
            //    var url = Android.Net.Uri.Parse(customPin.Url);
            //    var intent = new Intent(Intent.ActionView, url);
            //    intent.AddFlags(ActivityFlags.NewTask);
            //    Android.App.Application.Context.StartActivity(intent);
            //}
        }

        public Android.Views.View GetInfoContents(Marker marker)
        {
            var inflater = Android.App.Application.Context.GetSystemService(Context.LayoutInflaterService) as Android.Views.LayoutInflater;
            if (inflater != null)
            {
                Android.Views.View view;

                var customPin = GetCustomPin(marker);
                if (customPin == null)
                {
                    throw new Exception("Custom pin not found");
                }

                view = inflater.Inflate(Resource.Layout.MapInfoWindow, null);


                var infoTitle = view.FindViewById<TextView>(Resource.Id.InfoWindowTitle);
                var infoTime = view.FindViewById<TextView>(Resource.Id.Time);
                var infoDistance = view.FindViewById<TextView>(Resource.Id.Distance);
                var infoAqi = view.FindViewById<TextView>(Resource.Id.Aqi);
                var AqiFrame = view.FindViewById<FrameLayout>(Resource.Id.AqiFrame);

                if (infoTitle != null)
                {
                    infoTitle.Text = marker.Title;
                }
                if (infoAqi != null)
                {
                    infoAqi.Text = customPin.station.Aqi.ToString();
                }

                if (infoTime != null)
                {
                    infoTime.Text = "Aquired " +  (DateTime.UtcNow-customPin.station.Time.ToUniversalTime()).Duration().ToString();
                }
                if (infoDistance != null)
                {
                    infoDistance.Text = "Station is " + String.Format("{0:F1}", customPin.station.DistanceToUser) + " Km's away";
                }
                if (AqiFrame != null)
                {
                    var gDrawable = new GradientDrawable();
                    gDrawable.SetColor(customPin.station.Color.ToAndroid());
                    gDrawable.SetCornerRadius(10 * Resources.DisplayMetrics.Density + 0.5F);
                    AqiFrame.Background = gDrawable;
                }

                return view;
            }
            return null;
        }

        public Android.Views.View GetInfoWindow(Marker marker)
        {
            return null;
        }

        CustomPin GetCustomPin(Marker annotation)
        {
            var position = new Position(annotation.Position.Latitude, annotation.Position.Longitude);
            foreach (var pin in customPins)
            {
                if (pin.Position == position)
                {
                    return pin;
                }
            }
            return null;
        }
    }
}