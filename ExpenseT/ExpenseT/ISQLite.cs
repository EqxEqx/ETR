using System;
using SQLite;

namespace ExpenseT
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection();
    }
}