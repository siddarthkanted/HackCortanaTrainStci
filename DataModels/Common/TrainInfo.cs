using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.Common
{
    public class TrainInfo
    {
        public string Name;

        public string Number;

        public TrainInfo(string name, string number)
        {
            Name = name;
            Number = number;
        }

        public TrainInfo()
        {
        }
    }
}
