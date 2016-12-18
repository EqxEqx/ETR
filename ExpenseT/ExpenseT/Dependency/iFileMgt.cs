using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseT
{
    public interface IFileMgt
    {
        Boolean Delete(string fName);
        Boolean Rename(string fCurrent, string fRename, out string fRenameFull);
    }
}


