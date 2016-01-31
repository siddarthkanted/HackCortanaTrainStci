using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModels.Common;

namespace DataModels.Answer
{
    public class TrainBookingState : ITaskState
    {
        public TrainBookingState()
        {
            PassangerInfoList = new List<PassangerInfo>();
        }

        public Station Source;

        public Station Destination;

        public DateTime? DateOfJourney;

        public TrainInfo TrainInfo;

        public CoachType CoachPreference;

        public uint NumberOfPassangers;

        public List<PassangerInfo> PassangerInfoList;

        public string PhoneNumber;
    }
}
