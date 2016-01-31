using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.Common
{
    public class Station
    {
        public string Name;

        public string Code;

        public Station(string name, string code)
        {
            Name = name;
            Code = code;
        }

        public Station()
        {
        }
    }
}
