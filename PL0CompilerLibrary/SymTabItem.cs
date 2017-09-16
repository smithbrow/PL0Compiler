using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PL0CompilerLibrary
{
    public enum ObjectK
    {
        constant,
        variable,
        procedure
    };
    public class SymTabItem
    {
        public string   name { get; set; }

        public ObjectK  kind { get; set; }

        public int value { get; set; }

        public int level { get; set; }

        public int address { get; set; }

        public int size { get; set; }
    }
}
