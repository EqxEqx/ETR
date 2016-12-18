using FreshMvvm;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Reflection.Emit;
using System.Text;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

using PropertyChanged;   // Fody

namespace ExpenseT
{
    [ImplementPropertyChanged]   // Fody


    public class Page2PageModel : FreshMvvm.FreshBasePageModel
    {

        public ICommand ItemSelectedEditCommand { get; private set; }   // OpenExpenseItemPage
        public string SelectedItemText { get; private set; }


        //public List<LVCell> LVCellList { get; set; }

        public ObservableCollection<LVCell> LVCellList { get; set; }   // = new ObservableCollection<LVCell>();
        private int LVCellList_SelectedIndex = -1;

        public Page2PageModel()
        {
            try
            {
                // No code
            }
            catch (Exception ex)
            {       
                string eMsg = string.Format("Page2PageModel: {0}", ex.Message);
                CoreMethods.DisplayAlert("page2PageModel.cs", eMsg, "Cancel");

            }
        }


        public override void Init(object initData)
        {
            try
            {
                base.Init(initData);

                ItemSelectedEditCommand = new Command<LVCell>(OpenExpenseItemPage);

                // Load from DB
                LVCellList = new ObservableCollection<LVCell>();


                //  IEnumerable<ExpenseItem> dbRecs = App.Database.GetItems();   // Returns DB Identity in expenseItem.ID or -1 if error

                LVCell lvc = null;

                foreach (ExpenseItem eItem in App.Database.GetItems())
                {
                    lvc = new LVCell();

                    if (populateLVCell(eItem, lvc) == true)
                        LVCellList.Add(lvc);
                    else
                        lvc = null;

                }

            }
            catch (Exception ex)
            {
                string eMsg = string.Format("Init: {0}", ex.Message);
                CoreMethods.DisplayAlert("page2PageModel.cs", eMsg, "Cancel");
            }
        }

        /// <summary>
        /// Return from EDIT.  ( ExpenseItemPage.xaml )
        /// Paramter value maps to ExpenseItemAction class which has expense items fields.
        /// </summary>
        /// <param name="value"></param>
        public override void ReverseInit(object value)
        {
            string eMsg;

            try
            {
                // No Action
                if (value == null)
                    return;

                //***
                LVCell lvc = null;

                ExpenseItemAction eItemA = value as ExpenseItemAction;
                ExpenseItem eItem = eItemA.expenseItem;


                if(eItemA.action == "BACK")
                {
                    // Do nothing
                    return;
                }

                // DB Error
                if( eItem.ID <= 0 )
                {
                    eMsg = string.Format("Error: {0} failed writing to database.", eItemA.action);
                    return;
                }

                // Action triggered by ExpenseItemPage's button
                switch (eItemA.action)
                {
                    case "NEW":
                        // Create new cell
                        lvc = new LVCell();
                        LVCellList.Add(lvc);

                        break;


                    case "DELETE":                        
                        LVCellList.RemoveAt(LVCellList_SelectedIndex);
                        return;

                    case "UPDATE":
                        // Update existing cell
                        lvc = LVCellList[LVCellList_SelectedIndex];

                        break;

                    default:
                        eMsg = string.Format("Error: Unsupported {0}.", eItemA.action);
                        return;

                }

                //lvc.Header = string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}",
                //    eItem.ID, eItem.Name, eItem.Account,
                //    eItem.Category, eItem.Category2, eItem.Description, eItem.Amount);

                //lvc.Detail = string.Format("{0}, {1}", eItem.fName, eItem.fPath);

                populateLVCell(eItem, lvc);

            }
            catch( Exception ex )
            {
                CoreMethods.DisplayAlert("Page2PageModel:ReverseInit", ex.Message, "Error");
            }
        }


        private Boolean populateLVCell(ExpenseItem eItem, LVCell lvc)
        {
            try
            {
                lvc.Header = string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}",
                     eItem.Name, eItem.ExpenseDate, eItem.Amount, eItem.Account,
                     eItem.Category, eItem.Category2, eItem.ID);

                lvc.Detail = string.Format("{0}, {1}, {2}", eItem.Description, eItem.fName, eItem.fPath);

                return (true);
            }
            catch( Exception ex )
            {
                CoreMethods.DisplayAlert("Page2PageModel:populateLVCell", ex.Message, "Error");
                return (false);
            }
        }

             /// <summary>
        /// Edit existing Expense record.
        /// </summary>
        /// <param name="myCell"></param>
        async public void OpenExpenseItemPage(LVCell myCell)
        {
            try
            {
                // Pass Parameter


                // OnTapped causing 2nd call with myCell = NULL, then just return.

                if( myCell == null )
                    return;

                LVCellList_SelectedIndex = LVCellList.IndexOf(myCell);

                ExpenseItemAction eItemA = new ExpenseItemAction();
                ExpenseItem eItem = eItemA.expenseItem;

                string[] header = myCell.Header.Split(Convert.ToChar(","));
                string[] detail = myCell.Detail.Split(Convert.ToChar(","));

                eItemA.action = "EDIT";
            
                eItem.Name = header[0].Trim();
                eItem.ExpenseDate = header[1].Trim();

                // Amount
                decimal decVal;
                if (CommonMath.convert2Decimal(header[2], 2, out decVal) == false)
                {
                    await CoreMethods.DisplayAlert("Error: Conversion", "Expense Amount not valid", "Error");
                    return;
                }
                eItem.Amount = decVal;
                //

                eItem.Account = header[3].Trim();
                eItem.Category = header[4].Trim();
                eItem.Category2 = header[5].Trim();
                eItem.ID = Convert.ToInt32(header[6].Trim());

                // Line 2
                eItem.Description = detail[0].Trim();
                eItem.fName = detail[1].Trim();
                eItem.fPath = detail[2].Trim();

                await CoreMethods.PushPageModel<ExpenseItemPageModel>(eItemA, true, true);

            }
            catch  //(Exception ex)
            {
                // When OnTapped, this causes error.   TBD - Change to Behavior command
               // await CoreMethods.DisplayAlert("Page2PageModel:OpenExpenseItemPage", ex.Message, "Error");
            }
        }

        public Command cmdSelectedItemLVCell
        {
            get
            {
                return new Command(() =>
                {
                    string eMsg;

                    try
                    {

                        eMsg = string.Format("cmdSelectedItemLVCell: XAML SelectedItem");

                        CoreMethods.DisplayAlert("page2PageModel.cs", eMsg, "Cancel");

                        //CoreMethods.PushPageModel<UnicornPageModel>("Unicorn.png", true);

                        // ContactList.Add( new Contact { Name = "Sam", Number = "567" } );
                        //c.Name = "Sam";
                        //c.Number = "8787";

                        //ContactList.Add(c);

                        //            Xamarin.Forms.Core



                        //global::Xamarin.Forms.Label myLabel;
                        ////myLabel = this.ContactList.FindByName<global::Xamarin.Forms.Label>("myLabel");
                        //myLabel =  Xamarin.Forms.Element. FindByName<Label>("myLabel");


                        //var page = FreshPageModelResolver.ResolvePageModel<ContactListPageModel>();

                        //global::Xamarin.Forms.Label myLabel;
                        //myLabel = page.FindByName<Label>("myLabel");

                        //myLabel.Text = "AAAA";

                        //ContactList[1].Name = "jjjj";
                        //ContactList[1].Number = "0000";


                        //ContactList.Add(new Contact { Name = "Sam", Number = "567" });

                        //myLabelValue = "BBBB";

                    }

                    catch (Exception ex)
                    {

                        eMsg = string.Format("cmdSelectedLVCell: {0}", ex.Message);

                        CoreMethods.DisplayAlert("page2PageModel.cs", eMsg, "Cancel");
                    }

                });
            }
        }


        /// <summary>
        /// New Expense
        /// </summary>
        public Command NewItemCommand
        {
            get
            {
                return new Command(async() =>
                {
                    await CoreMethods.PushPageModel<ContactListPageModel> (null, true);
                });
            }
        }

        //public Command NewItemCommand
        //{
        //    get
        //    {
        //        return new Command(async () =>
        //        {
        //            try
        //            {
        //                // string myParm = "12;AB";
        //                //await CoreMethods.PushPageModel<ContactListPageModel>(myParm, true, true);

        //                await CoreMethods.PushPageModel<ContactListPageModel>();

        //            }
        //            catch (Exception ex)
        //            {
        //                string eMsg = ex.Message;
        //                await CoreMethods.DisplayAlert("NewItemCommand: Error", eMsg, "Close");
        //            }
        //        });   // return
        //    }  // get
        //} // public Command NewItemCommand


        //public Command NewItemCommand
        //{
        //    get
        //    {
        //        return new Command(() =>
        //        {
        //            try
        //            {
        //                //CoreMethods.DisplayAlert("NewItemCommand", "Pressed", "Close");

        //                newRecord();


        //                //var page = FreshPageModelResolver.ResolvePageModel<Page2PageModel>();
        //                //global::Xamarin.Forms.ListView myListView;
        //                //myListView = page.FindByName<ListView>("Page2ListView");
        //                //myListView.ItemsSource = App.Database.GetItems()        



        //                IEnumerable<ExpenseItem> iei = App.Database.GetItems();
        //                List<ExpenseItem> xx = iei.ToList<ExpenseItem>();

        //                LVCell lvc = new LVCell();
        //                lvc.Header = xx[0].ID.ToString();
        //                lvc.Detail = "ieiDetail";

        //                LVCellList.Add(lvc);



        //            }
        //            catch (Exception ex)
        //            {
        //                string eMsg = ex.Message;
        //                CoreMethods.DisplayAlert("NewItemCommand: Error", eMsg, "Close");
        //            }
        //        });   // return
        //    }  // get
        //} // public Command NewItemCommand


        //public void newRecord()
        //{

        //    try
        //    {
        //        ExpenseItem ei = new ExpenseItem();

        //        ei.ID = 0;
        //        ei.Name = "Jim";
        //        ei.Account = "Account";
        //        ei.Category = "Category";
        //        ei.Category2 = "Category2";
        //        ei.Amount = 12.34M;
        //        ei.Description = "Description";
        //        ei.fName = "fName";

        //        int rc = App.Database.SaveItem(ei);

        //       // CoreMethods.DisplayAlert("newRecord SaveItem: rc = ", rc.ToString(), "Close");

        //    }
        //    catch( Exception ex )
        //    {
        //        string eMsg = ex.Message;
        //        CoreMethods.DisplayAlert("newRecord: Error", eMsg, "Close");
        //    }
        //}

    }
}