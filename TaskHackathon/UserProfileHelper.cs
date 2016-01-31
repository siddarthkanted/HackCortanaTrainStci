using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DataModels.Answer;
using DataModels.Common;
using DataModels.Profile;

namespace TaskHackathon
{
    public class UserProfileHelper
    {

        public void UpdateUserProfile(TrainBookingState taskState, UserProfile userProfile)
        {
            // Update the passangers
            var knownPeopleList = userProfile.KnownPeopleList;

            if (knownPeopleList == null)
            {
                knownPeopleList = new List<Person>();
            }

            foreach (var passenger in taskState.PassangerInfoList)
            {
                if (passenger.Name.Equals(userProfile.UserId))
                {
                    Person me = userProfile.MyProfile;
                    if (string.IsNullOrEmpty(me.ContactNumber))
                    {
                        me.ContactNumber = taskState.PhoneNumber;
                    }
                    me.Gender = passenger.Gender;
                    me.DoB = DateTime.Now.AddYears(-(int) passenger.Age);
                }
                else
                {
                    Person person =
                        knownPeopleList.FirstOrDefault(
                            x => x.Name.Equals(passenger.Name, StringComparison.OrdinalIgnoreCase));
                    if (person == null)
                    {
                        person = new Person();
                    }

                    // TODO: do we need every time?
                    person.Name = passenger.Name;
                    person.Gender = passenger.Gender;
                    person.DoB = DateTime.Now.AddYears(-(int) passenger.Age);
                    person.Relation = passenger.Relation;
                    person.BerthChoice = passenger.BerthChoice;
                }
            }

            var preferredTrips = userProfile.PreferredTrips;
            if (preferredTrips == null)
            {
                preferredTrips = new Dictionary<string, List<TravelDestination>>();
                userProfile.PreferredTrips = preferredTrips;
            }

            List<TravelDestination> destinationList;
            string src = taskState.Source.Code;
            string dest = taskState.Destination.Code;
            if (!preferredTrips.TryGetValue(src, out destinationList))
            {
                destinationList = new List<TravelDestination>();
                preferredTrips.Add(src, destinationList);
            }

            TravelDestination destination = destinationList.FirstOrDefault(x => x.Code.Equals(dest));
            if (destination == null)
            {
                destination = new TravelDestination();
                destination.PreferredTrainList = new List<TrainInfo>();
                destination.Code = dest;
                destinationList.Add(destination);
            }

            TrainInfo trainInfo =
                destination.PreferredTrainList.FirstOrDefault(x => x.Number == taskState.TrainInfo.Number);

            if (trainInfo == null)
            {
                trainInfo = new TrainInfo();
                trainInfo.Number = taskState.TrainInfo.Number;
                trainInfo.Name = taskState.TrainInfo.Name;
                destination.PreferredTrainList.Add(trainInfo);
            }
        }




        // Update the trains

    }
}
