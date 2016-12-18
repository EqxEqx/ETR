using FreshMvvm;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace ExpenseT
{
    public partial class Page2Page : ContentPage
    {

        public Page2Page()
        {
            string eMsg;

            try
            {
                InitializeComponent();

            }
            catch (Exception ex)
            {
                eMsg = string.Format("Page2Page: {0}", ex.Message);
            }

        }

        /// <summary>
        /// Page2PageModel.Init runs first
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();
            // reset the 'resume' id, since we just want to re-start here
            //Page2ListView.ItemsSource = App.Database.GetItems();

        }

        /// <summary>
        /// When there is 1 item in the ListView, the "BACK" button returns to main screen but the item is selected; can not tap to open Edit function.
        /// This function Clears the ListView SelectedItem Row.
        /// Executed 2nd, After Command ItemSelected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void OnItemTapped(object sender, EventArgs args)
        {
            try
            {
                if (((ListView)sender).SelectedItem == null)
                    return;
                //Do stuff here with the SelectedItem ...
                ((ListView)sender).SelectedItem = null;
            }
            catch (Exception ex)
            {
                DisplayAlert("OnItemTapped", ex.Message, "Error");
            }
        }


    }
}
