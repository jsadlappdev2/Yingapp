﻿using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using Ying.Views;

namespace Ying
{

    public partial class Resource : ContentPage
    {
        ImageSource _imageSource;
        private IMedia _mediaPicker;
        Image image;
    

        public Resource()
        {
            Title = "Crop View";
            NavigationPage.SetHasNavigationBar(this, true);
            //ToolbarItems.Add(new ToolbarItem()
            //{
            //    Text = "Open",
            //    Order = ToolbarItemOrder.Primary,
            //    Command = new Command(async (x) =>
            //    {
            //        var action = await DisplayActionSheet(null, "Cancel", null, "Photo Library", "Take Photo");
            //        if (action == "Photo Library")
            //            await SelectPicture();
            //        else if (action == "Take Photo")
            //            await TakePicture();
            //        else if (action == "Cancel")
            //            return;
            //    }),
            //});

            image = new Image
            {
               Aspect = Aspect.AspectFit,
            };

            Label label = new Label
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                //VerticalTextAlignment = TextAlignment.Center,
                //HorizontalTextAlignment = TextAlignment.Center,
                Text = "Select an image to crop",
                TextColor = Color.Black
            };

            Image corpedimage = new Image {
                BackgroundColor = Color.Yellow,
                HeightRequest = 200,
                WidthRequest =200
               };
            if (App.CroppedImage != null)
            {
                Stream stream = new MemoryStream(App.CroppedImage);
                corpedimage.Source = ImageSource.FromStream(() => stream);
            }



                Button TPbutton = new Button
            {
                Text = "Take Photo",
                Font = Font.SystemFontOfSize(NamedSize.Large),
                BorderWidth = 1,
              //  HorizontalOptions = LayoutOptions.Center,
              //  VerticalOptions = LayoutOptions.CenterAndExpand
            };
            TPbutton.Clicked += async delegate
            {
               await TakePicture();           

            };

            Button PPbutton = new Button
            {
                Text = "Photo Library",
                Font = Font.SystemFontOfSize(NamedSize.Large),
                BorderWidth = 1,
                //  HorizontalOptions = LayoutOptions.Center,
                //  VerticalOptions = LayoutOptions.CenterAndExpand
            };
            PPbutton.Clicked += async delegate
            {
                await SelectPicture();              


            };


         




            BackgroundColor = Color.White;
            //   Content = label;
            this.Content = new StackLayout
            {
                Children =
                {
                    label,
                    corpedimage,
                   TPbutton,
                    PPbutton
                    
                }
            };

    
    }

        private void Refresh()
        {
            try
            {
                if (App.CroppedImage != null)
                {
                    Stream stream = new MemoryStream(App.CroppedImage);
                    image.Source = ImageSource.FromStream(() => stream);

                    Button Navbutton = new Button
                    {
                        Text = "Navigate to Reshource page",
                        Font = Font.SystemFontOfSize(NamedSize.Large),
                        BorderWidth = 1,
                        //  HorizontalOptions = LayoutOptions.Center,
                        //  VerticalOptions = LayoutOptions.CenterAndExpand
                    };
                    Navbutton.Clicked += async delegate
                    {
                        await Navigation.PushModalAsync(new Resource());


                    };

                    this.Content = new StackLayout
                    {
                        Children =
                            {
                                  Navbutton,
                                   image
                           }

                    };
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

       

        #region Photos

        private async void Setup()
        {
            if (_mediaPicker != null)
            {
                return;
            }

            ////RM: hack for working on windows phone? 
            await CrossMedia.Current.Initialize();
            _mediaPicker = CrossMedia.Current;
        }

        private async Task SelectPicture()
        {
            Setup();

            _imageSource = null;

            try
            {

                var mediaFile = await this._mediaPicker.PickPhotoAsync();

                _imageSource = ImageSource.FromStream(mediaFile.GetStream);

                var memoryStream = new MemoryStream();
                await mediaFile.GetStream().CopyToAsync(memoryStream);
                byte[] imageAsByte = memoryStream.ToArray();

              await Navigation.PushModalAsync(new CropView(imageAsByte, Refresh));
           

            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

         private async Task TakePicture()
        {
            Setup();

            _imageSource = null;

            try
            {
                var mediaFile = await this._mediaPicker.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    DefaultCamera = CameraDevice.Front
                });

                _imageSource = ImageSource.FromStream(mediaFile.GetStream);

                var memoryStream = new MemoryStream();
                await mediaFile.GetStream().CopyToAsync(memoryStream);
                byte[] imageAsByte = memoryStream.ToArray();

                await Navigation.PushModalAsync(new CropView(imageAsByte, Refresh));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        #endregion
    }
}