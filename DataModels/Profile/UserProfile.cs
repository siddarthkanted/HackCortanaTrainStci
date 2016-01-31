using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.Profile
{
    public class UserProfile
    {
        public string UserId;

        public Person MyProfile;

        public string HomeTown;

        public List<Person> KnownPeopleList;

        // <source, list of destinations>
        public Dictionary<string, List<TravelDestination>> PreferredTrips;

        public string LastSessionId;
    }
}
