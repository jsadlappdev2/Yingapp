using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using Ying.Views;


namespace Ying
{
    public class App : Application
    {

        public static byte[] CroppedImage;
        public App()
        {
            // The root page of your application
            // = new ContentPage
            //{
            //    Content = new StackLayout
            //    {
            //        VerticalOptions = LayoutOptions.Center,
            //        Children = {
            //             new Label {
            //                 HorizontalTextAlignment = TextAlignment.Center,
            //                 Text = "Welcome to Xamarin Forms!"
            //             }
            //         }
            //    }
            //};


          //  MainPage = new HomePage();
       
         MainPage = new NavigationPage(new HomePage { Title = "Learn Chinese with Ying" });


        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
