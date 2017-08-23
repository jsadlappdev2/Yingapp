using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
[assembly: Xamarin.Forms.Dependency(typeof(Ying.iOS.Picture_iOS))]
namespace Ying.iOS
{
    public class Picture_iOS : Ying.Interface.IPicture
    {
        public byte[] LoadPictureFromDisk(string filename)
        {
            byte[] fooResult = new byte[10];

            var documentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string filePath = System.IO.Path.Combine(documentsDirectory, filename);

            try
            {
                fooResult = System.IO.File.ReadAllBytes(filePath);
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e.ToString());
            }

            return fooResult;
        }

        public void SavePictureToDisk(string filename, byte[] imageData)
        {
            var documentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string filePath = System.IO.Path.Combine(documentsDirectory, filename);

            try
            {
                System.IO.File.WriteAllBytes(filePath, imageData);
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e.ToString());
            }
        }
    }

}