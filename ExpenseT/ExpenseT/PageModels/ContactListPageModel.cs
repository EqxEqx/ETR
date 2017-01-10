using PropertyChanged;
using FreshMvvm;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using System.IO;

namespace ExpenseT
{
    [ImplementPropertyChanged]
    public class ContactListPageModel : FreshMvvm.FreshBasePageModel
    {

        // Binded Xaml fields
        // fe:Picker
        public List<String> AccountList { get; set; }
        public String AccountSelected { get; set; }

        public List<String> CategoryList { get; set; }
        public String CategorySelected { get; set; }

        public List<String> Category2List { get; set; }
        public String Category2Selected { get; set; }

        //
        public List<Contact> ContactList { get; set; }

        public string Amount  { get; set; }
        public string Description   { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string Name { get; set; }

        private ImageSource imageSource;
        public ImageSource ImageSource
        {
            get { return imageSource; }
            set { imageSource = value; }
        }

        // END: Binded Xaml fields

        // Cache Main page's item values 
        private ExpenseItemAction reverseInitValueEI = new ExpenseItemAction();


        /*
         * CODE
         */

        public ContactListPageModel()
        {
            // Executes FIRST.  Then "Init"
            try
            {
                ExpenseDate = DateTime.Now;

                Name = "NA";
                AccountSelected = ""; // "DS";
                CategorySelected = ""; // "Mgt";
                Category2Selected = ""; // "Equipment";
                Description = "";
                Amount = "";

                ExpenseDate = DateTime.Now;
            }
            catch (Exception ex)
            {
                CoreMethods.DisplayAlert("ContactListPageModel", ex.Message, "Error");
            }
        }

        public override void Init(object initData)
        {
            try
            {
                // Executes Third - after ContactListPageModel()


                // fe:Picker
                AccountList = Common.AccountListInit();
                AccountSelected = "Common";

                CategoryList = Common.CategoryListInit();
                CategorySelected = "Mgt";

                Category2List = Common.Category2ListInit();
                Category2Selected = "Meal";


            }
            catch (Exception ex)
            {
                CoreMethods.DisplayAlert("Init", ex.Message, "Error");
            }
        }

        /// <summary>
        /// Pop page and return to Main.  Do nothing.
        /// </summary>
        public Command BackCommand
        {
            get
            {
                return new Command(async () =>
                {
                    reverseInitValueEI.action = "BACK";

                    Boolean brc1 = true;
                    Boolean brc2 = true;

                    if (reverseInitValueEI.expenseItem.fAlbumPath != "" && reverseInitValueEI.expenseItem.fAlbumPath != reverseInitValueEI.expenseItem.fPath)
                    {
                        brc1 = DependencyService.Get<IFileMgt>().Delete(reverseInitValueEI.expenseItem.fAlbumPath);
                    }

                    if (reverseInitValueEI.expenseItem.fPath != "")
                        brc2 = DependencyService.Get<IFileMgt>().Delete(reverseInitValueEI.expenseItem.fPath);

                    string delMsg = "";

                    if (brc1 == false && brc2 == false)
                        delMsg = string.Format("Error: Delete failed.  {0}, {1} ", reverseInitValueEI.expenseItem.fAlbumPath, reverseInitValueEI.expenseItem.fPath);
                    else if (brc1 == false)
                        delMsg = string.Format("Error: Delete failed. Album:  [{0}]", reverseInitValueEI.expenseItem.fAlbumPath);
                    else if (brc2 == false)
                        delMsg = string.Format("Error: Delete failed. File: [{0}]", reverseInitValueEI.expenseItem.fPath);


                    if (delMsg != "")
                        await CoreMethods.DisplayAlert("BAckCommand", delMsg, "Continue");

                    await CoreMethods.PopPageModel(null, true, true);
                });
            }
        }

        /// <summary>
        /// Receipt Photo.
        /// 
        /// </summary>
        public Command PhotoCommand
        {
            get
            {
                return new Command( async () =>
                {                   

                    string eMsg;

                    if( Description.Trim() == "")
                    {
                        await CoreMethods.DisplayAlert("Error", "Description Required for photo file name.", "NOP");
                        return;
                    }

                    try
                    {
                        /*
                         * ERROR    
                         */
                        await CrossMedia.Current.Initialize();

                    }
                    catch (Exception ex)
                    {
                        /*
                         * ERROR
                         * 
                         * This functionality is not implemented in the portable version of this assembly.  
                         * You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.
                         * 
                         * https://github.com/jamesmontemagno/MediaPlugin
                         * https://github.com/xamarin/Xamarin.Mobile
                         * 
                         */
                        eMsg = ex.Message;
                    }


                    if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                    {
                        await CoreMethods.DisplayAlert("No Camera", ":( No camera available.", "OK");
                        return;
                    }

                    /*
                     * Covert to byte[] - https://forums.xamarin.com/discussion/45925/pcl-xamarin-forms-convert-mediafile-into-byte-array
                     * Options: https://forums.xamarin.com/discussion/42600/how-to-take-picture-from-camera-and-retrieve-it-using-xamarin-forms-for-cross-platform-app
                     * StoreCameraMediaOptions
                     */

                    //string picName = DateTime.Now.ToString("yyyyMMddHHmmssff") + ".jpg";
                    //string DescriptionNoSpace = Description.Trim().Replace(' ', '_');
                    //string picName = string.Format("{0}_{1}.jpg", CommonDate.Convert2DateString(ExpenseDate, true), DescriptionNoSpace);

                    string picName;
                    bool brc = Common.createFileName(Description, ExpenseDate, "jpg", out picName);

                    try
                    {
                        // https://github.com/jamesmontemagno/MediaPlugin

                        var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                        {
                            Directory = "Expense",
                            SaveToAlbum = false,
                            PhotoSize = Plugin.Media.Abstractions.PhotoSize.Small,
                            Name = picName
                        });

                        if (file == null)
                            return;

                        // Expense info
                        ExpenseItem ei = reverseInitValueEI.expenseItem;

                        /*
                         * SaveToAlbum = true.  file.AlbumPath points to public gallery
                         * 
                         * fAlbumPath	/storage/emulated/0/Pictures/Expense/2016113013492249.jpg
                         * fPath	    /storage/emulated/0/Android/data/ExpenseT.Droid/files/Pictures/Expense/2016113013492249.jpg
                         * 
                         * SaveToAlbum = false. file.Path AND fAlbumPath point to private area.
                         * 
                         *  fPath, fAlbumPath   /storage/emulated/0/Android/data/ExpenseT.Droid/files/Pictures/Expense/2016113014120118.jpg 
                         */

                        ei.fAlbumPath = file.AlbumPath;
                        ei.fPath = file.Path;
                        ei.fName = picName;  // filename: 2016113014120118.jpg 

                        file.Dispose();

                        // Test 
                        ImageSource = ImageSource.FromFile(ei.fPath);

                        //  ImageSource = ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(str64)));
                        


                        string str64 = Utils.Base64Encode2String(ei.fPath);
                        ImageSource = Utils.Base64Decode2ImageSource(str64);


                    }
                    catch (Exception ex)
                    {
                        await CoreMethods.DisplayActionSheet("Photo", ex.Message, "Error");
                    }

                });    // return Command async

            } // get

        }  // PhotoCommand


            /// <summary>
            /// Save to DB and Return to Main page
            /// </summary>
        public Command SaveCommand
        {
            get
            {
                return new Command(async () =>
                {
                    // Save DB     
                    // New INSERT record.  reverseInitValueEI.ID = 0        

                    ExpenseItem ei = reverseInitValueEI.expenseItem;

                    reverseInitValueEI.action = "NEW";

                    ei.Name =  Name;
                    ei.Account = AccountSelected;
                    ei.Category = CategorySelected;  //  "Mgt";
                    ei.Category2 = Category2Selected;  //  "Travel";                 
                    ei.Description = Description;

                    ei.ExpenseDate = CommonDate.Convert2DateString(ExpenseDate, false);

                    decimal decVal; 
                    if( CommonMath.convert2Decimal(Amount, 2, out decVal) == false )
                    {
                        await CoreMethods.DisplayAlert("Error", "Amount not valid.", "Error");
                        return;
                    }

                    ei.Amount = decVal;

                    ei.CreatedDate = DateTime.Now.ToString("yyyy-MM-dd");

                    ei.ID = 0;    // 0 indicates NEW record.  INSERT.

                    // Convert image to string before saving to DB
                    ei.strImage64 = Utils.Base64Encode2String(ei.fName);

                    try
                    {
                        int irc = App.Database.SaveItem(reverseInitValueEI.expenseItem);   // Returns DB Identity in expenseItem.ID or -1 if error

                        if (irc <= 0)
                            ei.ID = irc;

                        // Return Main

                        //ei.strImage64 = "";   // Not needed. Clear and save memory. Retrieve from file or DB.   

                        await CoreMethods.PopPageModel(reverseInitValueEI, true, true);
                    }
                    catch( Exception ex )
                    {
                        await CoreMethods.DisplayAlert("Error", "DB Save failed. " + ex.Message, "Error");
                        return;
                    }
                    finally
                    {
                        ei.strImage64 = "";   // Not needed. Clear and save memory. Retrieve from file or DB.   

                    }

                });
            }
        }


    }
}
