using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebDB.Client.Prism.Core
{
    public static class GlobalCommands
    {
        public static CompositeCommand SaveAll = new CompositeCommand();
    }
}
