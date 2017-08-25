using System;
using System.IO;
using Android.Content;
using Android.Graphics;
using Com.Theartofdev.Edmodo.Cropper;

using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Ying;

[assembly: ExportRenderer(typeof(Ying.Views.CropView), typeof(Ying.Droid.Renderer.CropViewRenderer))]
namespace Ying.Droid.Renderer
{
    public class CropViewRenderer : PageRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);
            var page = Element as Ying.Views.CropView;
            if (page != null)
            {
                var cropImageView = new CropImageView(Context);
              
                cropImageView.LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);
                Bitmap bitmp = BitmapFactory.DecodeByteArray(page.Image, 0, page.Image.Length);
                cropImageView.SetImageBitmap(bitmp);

              //  var stackLayout = new StackLayout { Children = { cropImageView  } };

                var rotateButton = new Button { Text = "Rotate" };

                rotateButton.Clicked += (sender, ex) =>
                {
                    cropImageView.RotateImage(90);
                };
               // stackLayout.Children.Add(rotateButton);

                var finishButton = new Button { Text = "Finished" };
                finishButton.Clicked += (sender, ex) =>
                {
                    Bitmap cropped = cropImageView.CroppedImage;
                    using (MemoryStream memory = new MemoryStream())
                    {
                        cropped.Compress(Bitmap.CompressFormat.Png, 100, memory);
                        App.CroppedImage = memory.ToArray();
                    }
                    page.DidCrop = true;
                    page.Navigation.PopModalAsync();
                };

                //  stackLayout.Children.Add(finishButton);
                var stackLayout =
                             new StackLayout
                             {
                            Children =
                            {     rotateButton,
                                  finishButton,
                                  cropImageView

                           }

                           };
                page.Content = stackLayout;
            }
        }
    }
}

