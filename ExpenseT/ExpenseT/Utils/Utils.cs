using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ExpenseT
{
    public static class Utils
    {

        // Convert the Image bitmap into a Base64String and store it to SQLite.
        public static string Base64Encode(string fName)   // jpg file
        {

            Image image = new Image { Source = fName };
            string plainText = image.ToString();

            byte[] plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }


        // Retrieve: Fetch the Base64String and convert that to Bitmap again.
        public static string Base64Decode(string base64EncodedData)
        {
            byte[] base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);

            return (System.Text.Encoding.UTF8.GetString(base64EncodedBytes, 0, base64EncodedBytes.Length));
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
