using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ying.Interface
{
    public interface IPicture
    {
        void SavePictureToDisk(string filename, byte[] imageData);
        byte[] LoadPictureFromDisk(string filename);
    }

}
