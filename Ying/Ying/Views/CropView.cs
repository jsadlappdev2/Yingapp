﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.IO;
using System.Diagnostics;




namespace Ying.Views
{
    public class CropView : ContentPage
    {
        public byte[] Image;
        public Action RefreshAction;
        public bool DidCrop = false;

        public CropView(byte[] imageAsByte, Action refreshAction)
        {

            NavigationPage.SetHasNavigationBar(this, false);
            BackgroundColor = Color.Black;
            Image = imageAsByte;

            RefreshAction = refreshAction;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            if (DidCrop)
                RefreshAction.Invoke();
        }
    }
}
