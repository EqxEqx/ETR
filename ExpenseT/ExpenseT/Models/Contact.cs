using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;

namespace ExpenseT // .Models
{
    [ImplementPropertyChanged]
    public class Contact
    {
        public string Name { get; set;  }
        public string Number { get; set; }
    }
}
