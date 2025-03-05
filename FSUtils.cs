using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastScreener2
{
    internal class FSUtils
    {
  
        //for DPI
        public static Image ScaleImage(Image originalImage, float scaleFactor)
        {
            int newWidth = (int)(originalImage.Width * scaleFactor);
            int newHeight = (int)(originalImage.Height * scaleFactor);

            Bitmap resizedImage = new Bitmap(originalImage, new Size(newWidth, newHeight));
            return resizedImage;
        }
    }
}
