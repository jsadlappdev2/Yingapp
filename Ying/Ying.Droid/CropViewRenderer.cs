using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Ying.Views;
using Com.Theartofdev.Edmodo.Cropper;
using Android.Graphics;
using System.IO;

[assembly: ExportRenderer(typeof(CropView), typeof(Ying.Droid.CropViewRenderer))]
namespace Ying.Droid
{
    public class CropViewRenderer : PageRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);
            var page = Element as CropView;
            if (page != null)
            {
                var cropImageView = new CropImageView(Context);
                cropImageView.AutoZoomEnabled = false;
               // cropImageView.SetMinCropResultSize(200, 200);
               // cropImageView.SetMaxCropResultSize(300, 300);
              //  cropImageView.SetAspectRatio(1, 1);
             //   cropImageView.LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);
                Bitmap bitmp = BitmapFactory.DecodeByteArray(page.Image, 0, page.Image.Length);
                cropImageView.SetImageBitmap(bitmp);

              //  var stackLayout = new StackLayout { Children = { cropImageView } };

                var rotateButton = new Xamarin.Forms.Button { Text = "Rotate" };

                rotateButton.Clicked += (sender, ex) =>
                {
                    cropImageView.RotateImage(90);
                };
               // stackLayout.Children.Add(rotateButton);

                var finishButton = new Xamarin.Forms.Button { Text = "Finished" };
                finishButton.Clicked += (sender, ex) =>
                {
                    Bitmap cropped = cropImageView.CroppedImage;
                    using (MemoryStream memory = new MemoryStream())
                    {
                        cropped.Compress(Bitmap.CompressFormat.Png, 100, memory);
                        Ying.App.CroppedImage = memory.ToArray();
                    }
                    page.DidCrop = true;
                    page.Navigation.PopModalAsync();
                };

              //  stackLayout.Children.Add(finishButton);
              var stackLayout =
new StackLayout
{
    Children =
                            {    rotateButton,
finishButton,
                                  cropImageView
                               
                           }

};
                page.Content = stackLayout;
            }
        }
    }

}