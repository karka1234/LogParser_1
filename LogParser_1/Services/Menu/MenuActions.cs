﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogParser_1.Services.Menu
{
    internal abstract class MenuActions
    {
        public abstract void Action(List<Dictionary<string, object>> record);
    }
}
