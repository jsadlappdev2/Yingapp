using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Graphics.Drawables;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(TabbedPage), typeof(Ying.Droid.CustomTabRenderer))]
namespace Ying.Droid
{
    public class CustomTabRenderer:TabbedRenderer
    {
        private Activity _activity;

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            _activity = Context as Activity;
        }


        // May put this code in a different method - was just for testing
        //public override void OnWindowFocusChanged(bool hasWindowFocus)
        //{
        //    // Here the magic happens:  get your ActionBar and select the tab you want to add an image
        //    ActionBar actionBar = _activity.ActionBar;

        //    if (actionBar.TabCount > 0)
        //    {
        //        Android.App.ActionBar.Tab tabOne = actionBar.GetTabAt(0);

        //        tabOne.SetIcon(Resource.Drawable.ic_alarm_black);

        //    }
        //    base.OnWindowFocusChanged(hasWindowFocus);
        //}


        protected override void OnDraw(Android.Graphics.Canvas canvas)
        {
            ActionBar actionBar = ((Activity)this.Context).ActionBar;
            ColorDrawable colorDrawable = new ColorDrawable(Android.Graphics.Color.ParseColor("#87CEFA"));
            actionBar.SetStackedBackgroundDrawable(colorDrawable);
            if (actionBar.TabCount > 0)
            {
                ActionBar.Tab tabOne = actionBar.GetTabAt(0);
                tabOne.SetText("Photo");
                tabOne.SetIcon(Resource.Drawable.ic_photo_camera);




                ActionBar.Tab tabTwo = actionBar.GetTabAt(1);
                tabTwo.SetText("Speak");
                tabTwo.SetIcon(Resource.Drawable.ic_settings_voice);



                ActionBar.Tab tabThree = actionBar.GetTabAt(2);
                tabThree.SetText("Resource");
                tabThree.SetIcon(Resource.Drawable.ic_business_center);



                ActionBar.Tab tabFour = actionBar.GetTabAt(3);
                tabFour.SetText("Stroke");
                tabFour.SetIcon(Resource.Drawable.ic_text_fields);


                ActionBar.Tab tabFive = actionBar.GetTabAt(4);
                tabFive.SetText("Help");
                tabFive.SetIcon(Resource.Drawable.ic_help);


                //ActionBar.Tab tabfour = actionBar.GetTabAt(3);
                //tabfour.SetText("Resource");
                //tabfour.SetIcon(Resource.Drawable.ic_business_center);

                //ActionBar.Tab tabfive = actionBar.GetTabAt(4);
                //tabfive.SetText("Resource");
                //tabfive.SetIcon(Resource.Drawable.ic_business_center);


                //ActionBar.Tab tabsix = actionBar.GetTabAt(5);
                //tabsix.SetText("Resource");
                //tabsix.SetIcon(Resource.Drawable.ic_business_center);


                //ActionBar.Tab tabsev = actionBar.GetTabAt(6);
                //tabsev.SetText("Resource");
                //tabsev.SetIcon(Resource.Drawable.ic_business_center);


            }

            base.OnDraw(canvas);
        }

    }
}