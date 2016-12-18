using System;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;


namespace ExpenseT
{
    public class ExpenseItemDatabase : FreshMvvm.FreshBasePageModel
    {
        static object locker = new object();

        SQLiteConnection database;

        /// <summary>
        /// Initializes a new instance of the <see cref="Tasky.DL.TaskDatabase"/> TaskDatabase. 
        /// if the database doesn't exist, it will create the database and all the tables.
        /// </summary>
        /// <param name='path'>
        /// Path.
        /// </param>
        public ExpenseItemDatabase()
        {
            try
            {
                database = DependencyService.Get<ISQLite>().GetConnection();
                // create the tables
                database.CreateTable<ExpenseItem>();
            }
            catch( Exception ex )
            {
                CoreMethods.DisplayAlert("ExpenseItemDatabase", "Error creating database.  " + ex.Message, "Error");
            }
        }

        public IEnumerable<ExpenseItem> GetItems()
        {
            lock (locker)
            {                
                return (from i in database.Table<ExpenseItem>() select i).ToList();
            }
        }

        //public IEnumerable<ExpenseItem> GetItemsNotDone()
        //{
        //    lock (locker)
        //    {
        //        return database.Query<ExpenseItem>("SELECT * FROM [ExpenseItem] WHERE [Done] = 0");
        //    }
        //}

        public ExpenseItem GetItem(int id)
        {
            lock (locker)
            {
                return database.Table<ExpenseItem>().FirstOrDefault(x => x.ID == id);
            }
        }

        public int SaveItem(ExpenseItem item)
        {
            lock (locker)
            {
                try
                {
                    if (item.ID != 0)
                    {
                        database.Update(item);
                        return item.ID;
                    }
                    else
                    {
                        return database.Insert(item);
                    }
                }
                catch( Exception ex )
                {
                    string eMsg = ex.Message;                    
                    return (-1);
                }
            }
        }

        public int DeleteItem(int id)
        {
            lock (locker)
            {
                try
                {
                    int numDeleted = database.Delete<ExpenseItem>(id);
                    return(numDeleted);
                }
                catch ( Exception ex )
                {
                    string eMsg = ex.Message;
                    return (-1);
                }
            }
        }
    }
}


