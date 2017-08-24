
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Ying.Views;


namespace Ying
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Voice : ContentPage
    {
        ImageSource _imageSource;
        private IMedia _mediaPicker;
        Image image;

        public Voice()
        {
            InitializeComponent();

        }


        async void takephoto_click(object sender, EventArgs e)
        {
            await TakePicture();

        }


        async void pickupphoto_click(object sender, EventArgs e)
        {
            await SelectPicture();

        }


        private void Refresh()
        {
            try
            {
                if (App.CroppedImage != null)
                {
                    Image image = new Image
                    {
                        Aspect = Aspect.AspectFit,
                    };

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