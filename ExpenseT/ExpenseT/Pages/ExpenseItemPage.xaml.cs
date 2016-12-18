using FreshMvvm;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace ExpenseT
{
    public partial class ExpenseItemPage : ContentPage 
    {
        public ExpenseItemPage()
        {
            try
            {
                InitializeComponent();
            }
            catch( Exception ex)
            {
                DisplayAlert("ExpenseItemPage:InitializeComponent", ex.Message, "Error");
            }
        }

        //protected override void OnAppearing()
        //{
        //    base.OnAppearing();
            

        //    //imgReceipt.Source = ImageSource.FromFile()

        //}
    }
}
