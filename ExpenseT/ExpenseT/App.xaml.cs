using FreshMvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace ExpenseT
{
    public partial class App : Application
    {

        static ExpenseItemDatabase database;


        public App()
        {
            try
            {
                //MainPage = FreshPageModelResolver.ResolvePageModel<ContactListPageModel>();

                OnePage();

                //LoadBasicNav();
                //LoadMasterDetail();
                //LoadTabbedNav();

            }
            catch ( Exception ex )
            {
                string eMsg = ex.Message;
            }
        }



        public void OnePage()
        {
            try
            {
                // MainPage = FreshPageModelResolver.ResolvePageModel<Page2PageModel>();
                // MainPage = FreshPageModelResolver.ResolvePageModel<ContactListPageModel>();

                var page = FreshPageModelResolver.ResolvePageModel<Page2PageModel>();
                var basicNavContainer = new FreshNavigationContainer(page);
                MainPage = basicNavContainer;

            }
            catch (Exception ex)
            {
                string eMsg = ex.Message;
            }
        }

        public void LoadBasicNav()
        {
            var page = FreshPageModelResolver.ResolvePageModel<ContactListPageModel>();
            var basicNavContainer = new FreshNavigationContainer(page);
            MainPage = basicNavContainer;
        }

        public void LoadMasterDetail()
        {

            try
            {
                var masterDetailNav = new FreshMasterDetailNavigationContainer();
                //masterDetailNav.Init("Menu", "Menu.png");
                masterDetailNav.Init("Menu", null);
                masterDetailNav.AddPage<ContactListPageModel>("ContactList", null);


                masterDetailNav.AddPage<Page2PageModel>("Page2", null);
                //masterDetailNav.AddPage<ContactListPageModel>("Page2", null);

                MainPage = masterDetailNav;
            }
            catch (Exception ex)
            {
                string eMsg = ex.Message;
            }

        }

        public void LoadTabbedNav()
        {

            try
            {
                var tabbedNavigation = new FreshTabbedNavigationContainer();
                //tabbedNavigation.AddTab<ContactListPageModel>("Contacts", "contacts.png", null);
                //tabbedNavigation.AddTab<Page2PageModel>("Quotes", "document.png", null);

                tabbedNavigation.AddTab<ContactListPageModel>("ContactList", null, null);
                tabbedNavigation.AddTab<Page2PageModel>("Page2", null, null);

                MainPage = tabbedNavigation;
            }
            catch (Exception ex)
            {
                string eMsg = ex.Message;
            }
}

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        //****************************
        public static ExpenseItemDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new ExpenseItemDatabase();
                }
                return database;
            }
        }


    }  // partial class App : Application
}  // namespace ExpenseT
