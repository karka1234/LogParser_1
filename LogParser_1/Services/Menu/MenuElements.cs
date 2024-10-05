using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogParser_1.Services.Menu
{
    internal abstract class MenuElements
    {
        public abstract void PrintMenu(string status);
        public abstract void Action(List<Dictionary<string, object>> record, out string statusString);
        //public abstract Task ActionAsync(List<Dictionary<string, object>> record);
    }
}
