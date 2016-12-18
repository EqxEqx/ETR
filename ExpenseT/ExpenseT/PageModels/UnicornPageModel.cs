using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace ExpenseT
{
    public class UnicornPageModel : FreshMvvm.FreshBasePageModel
    {
        public string ImagePath { get; private set; }

        public override void Init(object initData)
        {
            base.Init(initData);

            var imagePath = initData as string;

            if (string.IsNullOrWhiteSpace(imagePath))
            {
                CoreMethods.DisplayAlert("Error!", "Not an image path!", "OK");
                return;
            }

            ImagePath = imagePath;
            RaisePropertyChanged("ImagePath");
        }
    }
}
