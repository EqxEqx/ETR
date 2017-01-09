using PropertyChanged;
using FreshMvvm;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Reflection.Emit;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

using Plugin.Messaging;

namespace ExpenseT
{
    [ImplementPropertyChanged]
    public class ExpenseItemPageModel : FreshBasePageModel
    {
        // Binded fields

        // fe:Picker
        public List<String> AccountList { get; set; }
        public String AccountSelected { get; set; }   // Picker value

        public List<String> CategoryList { get; set; }
        public String CategorySelected { get; set; }  // Picker value

        public List<String> Category2List { get; set; }
        public String Category2Selected { get; set; }  // Picker value

        //
        public ExpenseItemAction expenseItemAction;   // Passed in values from Main screen

        public string Amount { get; set; }   // Expense amount
        public string Description { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string Name { get; set; }

        private ImageSource imageSource;  // Image
        public ImageSource ImageSource
        {
            get
            {
                return imageSource;
            }
            set
            {
                imageSource = value;
            }
        }

        // END - Binded fields

       // Boolean InitAlreadyRun = false;   // Stop Init function from being run twice


            /*
             * Code
             */

        /// <summary>
        /// Called from Main.
        /// objExpenseItemAction - ListView selected item for editing.
        /// </summary>
        /// <param name="objExpenseItemAction"></param>
        public override void Init(object objExpenseItemAction)
        {   
            try
            {

                //if (InitAlreadyRun == true)
                //    return;

                // https://forums.xamarin.com/discussion/19853/load-image-form-byte-array

                //base.Init(objExpenseItem);
                expenseItemAction = objExpenseItemAction as ExpenseItemAction;   // Item saved in class's global variable declared above

                ExpenseItem expenseItem = expenseItemAction.expenseItem;  // Easier reference 
             
                // Put on screen. Map to screen's binded fields.   
                Amount = expenseItem.Amount.ToString();
                Description = expenseItem.Description;
                ExpenseDate = CommonDate.Convert2DateTime(expenseItem.ExpenseDate);
                Name = expenseItem.Name;

                // TEST - Read from DB
                //ImageSource = ImageSource.FromFile(expenseItem.fPath);

                // TBD 1
                string ImageBase64 = "";
                ImageSource = ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(ImageBase64)));





                // fe:Picker
                AccountList = Common.AccountListInit();  // Initialize Picker
                AccountSelected = expenseItem.Account;              // Binded field

                CategoryList = Common.CategoryListInit();
                CategorySelected = expenseItem.Category;

                Category2List = Common.Category2ListInit();
                Category2Selected = expenseItem.Category2;

             //   InitAlreadyRun = true;


            }
            catch (Exception ex)
            {
                string eMsg = string.Format("Init: {0}", ex.Message);

                CoreMethods.DisplayAlert("page2PageModel.cs", eMsg, "Cancel");
            }
        }

        /// <summary>
        /// Pop page and return to Main.  Do nothing.
        /// </summary>
        public Command BackCommand
        {
            get
            {
                return new Command( async () =>
                {
                    expenseItemAction.action = "BACK";

                    await CoreMethods.PopPageModel(null, true, true);
                });
            }
        }

        /// <summary>
        /// Delete item.
        /// </summary>
        public Command DeleteCommand
        {
            get
            {
                return new Command(async () =>
                {

                    int numDeleted = App.Database.DeleteItem(expenseItemAction.expenseItem.ID);   // -1 if Error

                    if( numDeleted <= 0 )
                    {
                        expenseItemAction.action = "BACK";  // Do not update Main's item

                        await CoreMethods.DisplayAlert("Error", "Delete failed.", "Error");
                    }

                    // Delete files

                    Boolean brc1 = true;
                    Boolean brc2 = true;

                    if (expenseItemAction.expenseItem.fAlbumPath != "" && expenseItemAction.expenseItem.fAlbumPath != expenseItemAction.expenseItem.fPath)
                    {
                        brc1 = DependencyService.Get<IFileMgt>().Delete(expenseItemAction.expenseItem.fAlbumPath);
                    }

                    if (expenseItemAction.expenseItem.fPath != "")
                        brc2 = DependencyService.Get<IFileMgt>().Delete(expenseItemAction.expenseItem.fPath);

                    string delMsg = "";

                    if (brc1 == false && brc2 == false)
                        delMsg = string.Format("Error: Delete failed.  {0}, {1} ", expenseItemAction.expenseItem.fAlbumPath, expenseItemAction.expenseItem.fPath);
                    else if (brc1 == false)
                        delMsg = string.Format("Error: Delete failed. Album:  [{0}]", expenseItemAction.expenseItem.fAlbumPath);
                    else if (brc2 == false)
                        delMsg = string.Format("Error: Delete failed. File: [{0}]", expenseItemAction.expenseItem.fPath);


                    if (delMsg != "")
                        await CoreMethods.DisplayAlert("DeleteCommand", delMsg, "Continue");

                    expenseItemAction.action = "DELETE";

                    await CoreMethods.PopPageModel(expenseItemAction, true, true);
                });
            }
        }


        /// <summary>
        /// Scrape screen for fields values and return.   Main function will UPDATE database.
        /// </summary>
        public Command UpdateCommand
        {
            get
            {
                return new Command(async () =>
                {
                    // Scrape screen for fields values.
                    // expenseItemAction is class global field
                    
                    ExpenseItem expenseItem = expenseItemAction.expenseItem;

                    expenseItemAction.action = "UPDATE";
                    expenseItem.Name = Name;
                    expenseItem.Account = AccountSelected;

                    expenseItem.ExpenseDate = CommonDate.Convert2DateString(ExpenseDate, false);

                    decimal decVal;

                    if( CommonMath.convert2Decimal( Amount, 2, out decVal) == false )
                    {
                        await CoreMethods.DisplayAlert("Update Error", "Amount not valid decimal value.", "Error");
                        return;
                    }

                    expenseItem.Amount = decVal;

                    expenseItem.Category = CategorySelected;
                    expenseItem.Category2 = Category2Selected;
                    expenseItem.Description = Description;


                    // Possible file rename
                    string fName;
                    string fRenameFull;   // full path and filename
                    bool brc = Common.createFileName(Description, ExpenseDate, "jpg", out fName);
                    brc = DependencyService.Get<IFileMgt>().Rename(expenseItem.fPath, fName, out fRenameFull);

                    // Update - Photo file renamed
                    expenseItem.fName = fName;
                    expenseItem.fPath = fRenameFull;

                    expenseItem.CreatedDate = DateTime.Now.ToString("yyyy-MM-dd");

                    //expenseItem.ID > 0 indicate UPDATE
                    expenseItem.ID = App.Database.SaveItem(expenseItem);   // -1 if Error

                    await CoreMethods.PopPageModel(expenseItemAction, true, true);
                });
            }
        }

        /// <summary>
        /// EMail fields as CSV and image as attachment
        /// </summary>
        public Command eMailCommand
        {
            get
            {
                return new Command( () =>
                {
                    //await CoreMethods.PushPageModel<ContactListPageModel>(null, true);


                    var emailTask = MessagingPlugin.EmailMessenger;
                    if (emailTask.CanSendEmail)
                    {
                        ExpenseItem expenseItem = expenseItemAction.expenseItem;

                        string subject = string.Format("E: {0}, {1}, {2}, {3}, {4}, {5}",
                            expenseItem.Name, expenseItem.Account, expenseItem.ExpenseDate, expenseItem.Amount, expenseItem.Category, expenseItem.Category2);

                        string msg = string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}",
                            expenseItem.ID, expenseItem.Name, expenseItem.Account, expenseItem.Category, expenseItem.Category2,
                            expenseItem.ExpenseDate, expenseItem.Amount, expenseItem.Description, expenseItem.fName);

                        IEmailMessage email;

                        if (expenseItem.fPath == "")
                        {
                            email = new EmailMessageBuilder()
                             .To("equinoxc@hotmail.com")
                             //  .Cc("equinoxmc@hotmail.com")
                             //  .Bcc(new[] { "plugins.bcc@xamarin.com", "plugins.bcc2@xamarin.com" })
                             .Subject(subject)
                             .Body(msg)
                             .Build();
                        }
                        else
                        {                        
                           email = new EmailMessageBuilder()
                             .To("equinoxc@hotmail.com")
                             //  .Cc("equinoxmc@hotmail.com")
                             //  .Bcc(new[] { "plugins.bcc@xamarin.com", "plugins.bcc2@xamarin.com" })
                             .Subject(subject)
                             .Body(msg)
                             .WithAttachment(expenseItem.fPath, "image/jpeg")
                             .Build();
                        }

                        emailTask.SendEmail(email);

                    }
                    else
                    {
                        CoreMethods.DisplayAlert("Not Supported", "Email can not be run from this device", "Close");
                    }
                });
            }
        }


    }
}
