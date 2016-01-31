using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.Profile
{
    public class TravelDestination
    {
        public string Code;

        // Tuple<trainname, train number>
        public List<Tuple<string, uint>> PreferredTrainList;
    }
}
