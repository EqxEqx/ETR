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
    public partial class ContactListPage : ContentPage
    {


        public ContactListPage()
        {
            // Executes 2nd after ContactListPageModel

            try
            { 
                InitializeComponent();
               // this.BindingContext = new ContactListPageModel();
            }
            catch (Exception ex)
            {
                DisplayAlert("ContactListPage:InitializeComponent", ex.Message, "Error");
            }

        }

        //async void Photo_OnClicked(object sender, EventArgs e)
        //{
        //    await CrossMedia.Current.Initialize();

        //    if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
        //    {
        //        DisplayAlert("No Camera", ":( No camera available.", "OK");
        //        return;
        //    }


        //    var page = FreshPageModelResolver.ResolvePageModel<ContactListPageModel>();

        //    //global::Xamarin.Forms.Label myLabel;
        //    //myLabel = page.FindByName<Label>("myLabel");



        //    var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
        //    {
        //        Directory = "Sample",
        //        Name = "test.jpg"
        //    });

        //    if (file == null)
        //        return;

        //    await DisplayAlert("File Location", file.Path, "OK");



        //    //global::Xamarin.Forms.Image image;
        //    //image = page.FindByName<Image>("image");
        //    //myimage.Source = ImageSource.FromStream(() =>
            
        //    myimage.Source = ImageSource.FromStream(() =>
        //    {
        //        var stream = file.GetStream();
        //        file.Dispose();
        //        return stream;
        //    });

        //}

        //void Photo_OnClicked(object sender, EventArgs e)
        //{

        //    myimage.Source = "icon.png";

        //}

    }
}
