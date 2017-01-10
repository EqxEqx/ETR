using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xamarin.Forms;

namespace ExpenseT
{
    public static class Utils
    {
        // Convert the Image bitmap into a Base64String and store it to SQLite.
        public static string Base64Encode2String(string fName)   // jpg file
        {
            try
            {
                // http://stackoverflow.com/questions/37879437/convert-file-to-byte-array-given-its-path-in-local-storage

                byte[] b = DependencyService.Get<IFileMgt>().convertImage2ByteArray(fName);

                return (System.Convert.ToBase64String(b));

                //using (var streamReader = new StreamReader(fName))
                //{
                //    using (var memoryStream = new MemoryStream())
                //    {
                //        streamReader.BaseStream.CopyTo(memoryStream);
                //        byte[] plainTextBytes =  memoryStream.ToArray();
                //        return System.Convert.ToBase64String(plainTextBytes);
                //    }

                //}
            }
            catch( Exception ex)
            {
                string eMsg = ex.Message;
                return (null);
            }

        }

        //public static string Base64Encode2(string fName)   // jpg file
        //{




        //    Image image = new Image { Source = fName };

        //    string plainText = image.ToString();

        //    byte[] plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        //    return System.Convert.ToBase64String(plainTextBytes);
        //}


        // Retrieve: Fetch the Base64String and convert that to Bitmap again.

        public static byte[] Base64Decode2Byte(string base64EncodedData)
        {
            byte[] base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);

            return (base64EncodedBytes);
        }

        public static string Base64Decode2String(string base64EncodedData)
        {
            byte[] base64EncodedBytes = Base64Decode2Byte(base64EncodedData);

            return (System.Text.Encoding.UTF8.GetString(base64EncodedBytes, 0, base64EncodedBytes.Length));
        }

        public static MemoryStream Base64Decode2MemoryStream(string base64EncodedData)
        {
            byte[] base64EncodedBytes = Base64Decode2Byte(base64EncodedData);

            MemoryStream ms = new MemoryStream(base64EncodedBytes);

            return (ms);
        }

        public static ImageSource Base64Decode2ImageSource(string base64EncodedData)
        {
            try
            {
                //  ImageSource = ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(str64)))

                ImageSource img = ImageSource.FromStream(() => Base64Decode2MemoryStream(base64EncodedData));

                return (img);
            }
            catch (Exception ex)
            {
                string eMsg = ex.Message;
                return (null);
            }
        }

        /*
         * xamarin forms c# display base64string image
         * http://stackoverflow.com/questions/37080258/xamarin-show-image-from-base64-string
         * 
         * c# display base64string image
         * https://www.google.com/search?q=c%23+display+base64string+image&ie=utf-8&oe=utf-8
         * http://stackoverflow.com/questions/18827081/c-sharp-base64-string-to-jpeg-image
         * 
         * 
         * 
         * 
         * 
         */

    }
}
