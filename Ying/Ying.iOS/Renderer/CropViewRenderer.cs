using System;
using System.Diagnostics;
using Foundation;
using UIKit;
using Xam.Plugins.ImageCropper.iOS;
using Ying;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Ying.Views.CropView), typeof(Ying.iOS.Renderer.CropViewRenderer))]
namespace Ying.iOS.Renderer
{

    public class CropViewRenderer : PageRenderer
    {
        CropViewDelegate selector;
        byte[] Image;
        bool IsShown;
        public bool DidCrop;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var page = base.Element as Ying.Views.CropView;
            Image = page.Image;
            DidCrop = page.DidCrop;
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            try
            {
                if (!IsShown)
                {

                    IsShown = true;

                    UIImage image = new UIImage(NSData.FromArray(Image));
                    Image = null;

                    selector = new CropViewDelegate(this);

                    TOCropViewController picker = new TOCropViewController(image);
                    // Demo for Circular Cropped Image
                    //TOCropViewController picker = new TOCropViewController(TOCropViewCroppingStyle.Circular, image);
                    picker.Delegate = selector;

                    PresentViewController(picker, false, null);

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            try
            {
                var page = base.Element as Ying.Views.CropView;
                page.DidCrop = selector.DidCrop;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }
    }


    public class CropViewDelegate : TOCropViewControllerDelegate
    {
        readonly UIViewController parent;
        public bool DidCrop;

        public CropViewDelegate(UIViewController parent)
        {
            this.parent = parent;
        }

        public override void DidCropToImage(TOCropViewController cropViewController, UIImage image, CoreGraphics.CGRect cropRect, nint angle)
        {
            DidCrop = true;

            try
            {
                if (image != null)
                    App.CroppedImage = image.AsPNG().ToArray();

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                if (image != null)
                {
                    image.Dispose();
                    image = null;
                }
            }

            parent.DismissViewController(true, () => { App.Current.MainPage.Navigation.PopModalAsync(); });
        }

        public override void DidFinishCancelled(TOCropViewController cropViewController, bool cancelled)
        {
            parent.DismissViewController(true, () => { App.Current.MainPage.Navigation.PopModalAsync(); });
        }
    }
}

