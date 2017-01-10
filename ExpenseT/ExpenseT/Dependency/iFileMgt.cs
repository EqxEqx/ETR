using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseT
{
    public interface IFileMgt
    {
        // Delete file
        Boolean Delete(string fName);

        // Rename file
        Boolean Rename(string fCurrent, string fRename, out string fRenameFull);

        // Convert image ( jpg ) to byte array
        byte[] convertImage2ByteArray(string fName);
    }
}


