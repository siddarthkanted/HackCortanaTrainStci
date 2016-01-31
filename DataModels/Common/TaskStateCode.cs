using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.Common
{
    public enum TaskStateCode
    {
        InitState,
        SourceStationState,
        DestinationStationState,
        DateOfJourneyState,
        TrainSelectionState,
        PreferredCoachState,
        NumberOfPassangersState,
        PassengerNameState,
        PassengerAgeState,
        PassengerGenderState,
        PassengerSeatPreferenceState,
        PassengerRelationState,
        PassengerDetailsState,
        PhoneNumberState,
        BookTicketState,
        SaveState,
        ExceptionState,
        EndState
    }
}
