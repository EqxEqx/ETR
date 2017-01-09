using System;
using SQLite;
using Xamarin.Forms;

namespace ExpenseT
{

    public class ExpenseItem
    {

        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Account { get; set; }    // SF, DC, NY
        public string Category { get; set; }   // Mgt, Travel
        public string Category2 { get; set; }  // Supplies, Tools, Food, Gas
        public string ExpenseDate { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string fAlbumPath { get; set; }  // Picture album reference
        public string fPath { get; set; }  // Full path and filename  /abc/def/pic1.jpb
        public string fName { get; set; }  // Just filename   pic1.jpg
        public string CreatedDate { get; set; } // Date entered
        // Used by ExpenseMgt.  Converts to PDF
        public string fPDF { get; set; }
        public string dirfPDF { get; set; }

        public string strImage64 { get; set; }


        public ExpenseItem()
        {
            Clear();            
        }

        public void Clear()
        {
            ID = 0;
            Name = "";
            Account = "";
            Category = "";
            Category2 = "";
            Amount = 0.0M;
            Description = "";
            fAlbumPath = "";
            fPath = "";
            fName = "";
            dirfPDF = "";
            strImage64 = "";  // Image saved in DB.  Reduces the Azure sync code.
        }
    }

    public class ExpenseItemAction
    {
        public string action { get; set; }
        public ExpenseItem expenseItem { get; set; }

        public ExpenseItemAction()
        {
            action = "";
            expenseItem = new ExpenseItem();
        }
    }


}
