using System;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace SCCO.WPF.MVC.CS.Utilities {

    public class ImageTool {

        public static byte[] GetBytesFromImageFile(string imageFileName)
        {
            if (!File.Exists(imageFileName)) return null;

            byte[] bytes;
            FileStream fs = null;
            BinaryReader br = null;
            try
            {
                fs = new FileStream(imageFileName, FileMode.Open, FileAccess.Read);
                br = new BinaryReader(fs);
                bytes = br.ReadBytes((int)fs.Length);
            }
            finally
            {
                if(br != null)
                    br.Close();
                if (fs != null)
                    fs.Close();
            }
            //using (Image image = Image.FromFile(imageFileName))
            //{
            //    using (var memoryStream = new MemoryStream())
            //    {
            //        image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            //        bytes = memoryStream.ToArray();
            //    }
            //}

            return bytes;
        }

        public static Bitmap GetBitmapFromBytes(byte[] imageBytes)
        {
            if (imageBytes == null) return null;
            try
            {
                return (Bitmap)Image.FromStream(new MemoryStream(imageBytes));
            }
            catch (Exception exception)
            {
                Logger.ExceptionLogger(new ImageTool(), exception);
            }
            return null;
        }

        public static BitmapImage CreateImageSourceFromBytes(byte[] imageBytes)
        {
            if (imageBytes == null || imageBytes.Length == 0) return null;
            try
            {
                var imageSource = new BitmapImage();
                imageSource.BeginInit();
                imageSource.StreamSource = new MemoryStream(imageBytes);
                imageSource.EndInit();
                return imageSource;
            }
            catch (Exception exception)
            {
                Logger.ExceptionLogger(new ImageTool(), exception);
            }
            return null;
        }

        public static BitmapImage CreateImageSourceFromImageFile(string imageFileName) {
            if (!File.Exists(imageFileName)) return null;
            try {
                var imageSource = new BitmapImage();
                imageSource.BeginInit();
                imageSource.StreamSource = File.OpenRead(imageFileName);
                imageSource.EndInit();
                return imageSource;
            } catch (Exception exception) {
                Logger.ExceptionLogger(new ImageTool(), exception);
            }
            return null;
        }
    }
}
