using System;
using System.Drawing;
using System.IO;
using ExpenseT;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileMgt_Android))]

namespace ExpenseT
{
    public class FileMgt_Android : IFileMgt
    {
        public FileMgt_Android()
        {
        }

        public Boolean Delete( string fName )
        {
            Boolean brc = false;
            try
            {
                if (File.Exists(fName))
                {
                    File.Delete(fName);
                    brc = true;
                }

                return (brc);

            }
            catch( Exception ex)
            {
                string eMsg = ex.Message;         
                return (false);
            }
        }

        /// <summary>
        /// Renames file.
        /// fCurrent: /a/b/c/fName.jpg ( Full directory path and file name )
        /// fRename: fRenamed.jpg ( Just new file name without directory path )
        /// Renamed fiie is /a/b/c/fRenamed.jpg
        /// </summary>
        /// <param name="fCurrnet"></param>
        /// <param name="fRename"></param>
        /// <returns></returns>
        public Boolean Rename(string fCurrent, string fRename, out string fRenameFull)
        {
            fRenameFull = "";

            Boolean brc = false;
            try
            {
                if (File.Exists(fCurrent))
                {
                    // Find last "/" separator

                    int iFound = -1;

                    for (int ia = fCurrent.Length - 1; ia >= 0; ia--)
                    {
                        if( fCurrent[ia] == '/' )
                        {
                            iFound = ia;
                            break;
                        }
                    }
                    
                    if( iFound >= 0 )
                    {
                        // Directory path including last "/" separator.   /a/b/
                        fRenameFull = fCurrent.Substring(0, iFound + 1);   // index is zero based
                    }

                    // /a/b/fRname.jpg
                    fRenameFull += fRename;


                    File.Move(fCurrent, fRenameFull);

                    brc = true;
                }

                return (brc);

            }
            catch (Exception ex)
            {
                string eMsg = ex.Message;
                return (false);
            }
        }

        /// <summary>
        /// Convert image file to byte array.
        /// </summary>
        /// <param name="fName">Path and filename</param>
        /// <returns></returns>
        public byte[] convertImage2ByteArray(string fName)
        {

            // http://stackoverflow.com/questions/37879437/convert-file-to-byte-array-given-its-path-in-local-storage

            try
            {
                using (var streamReader = new System.IO.StreamReader(fName))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        streamReader.BaseStream.CopyTo(memoryStream);
                        return( memoryStream.ToArray() );
                    }

                }
            }
            catch( Exception ex)
            {
                string eMsg = ex.Message;
                return (null);
            }
        }



        // http://stackoverflow.com/questions/17352061/fastest-way-to-convert-image-to-byte-array?noredirect=1&lq=1
        public static byte[] ReadImageFile(string imageLocation)
        {
            byte[] imageData = null;
            FileInfo fileInfo = new FileInfo(imageLocation);
            long imageFileLength = fileInfo.Length;
            FileStream fs = new FileStream(imageLocation, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            imageData = br.ReadBytes((int)imageFileLength);
            return imageData;
        }




        //public string Base64EncodeImage( string fName)
        //{
        //    try
        //    {

        //        if (File.Exists(fName) == false)
        //        {
        //            return ("");
        //        }

        //        Image image = Image.(fName);
        //        {
        //            using (MemoryStream m = new MemoryStream())
        //            {
        //                image.Save(m, image.RawFormat);
        //                byte[] imageBytes = m.ToArray();

        //                // Convert byte[] to Base64 String
        //                string base64String = Convert.ToBase64String(imageBytes);
        //                return base64String;
        //            }
        //        }


        //    }
        //    catch( Exception ex)
        //    {
        //        return ("");
        //    }
        //}
    }
}