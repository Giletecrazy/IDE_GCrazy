using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IDE_GCrazy
{
    public class AnaliserList
    {
        public string type { get; set; }
        public int index { get; set; }
        public string value { get; set; }

        public AnaliserList(string type, int index, string value)
        {
            this.type = type;
            this.index = index;
            this.value = value;
        }
    }
}
