using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;

namespace ExpenseT
{
    [ImplementPropertyChanged]
    public class LVCell
    {
        public string Header { get; set; }
        public string Detail { get; set; }
    }
}
