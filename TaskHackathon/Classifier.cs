using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DataModels.Answer;
using DataModels.Common;
using DataModels.Profile;

namespace TaskHackathon
{
    public class Classifier
    {
        //public static ConcurrentDictionary<string, QueryResponse> queryResponseDict = new ConcurrentDictionary<string, QueryResponse>();
        public Dictionary<string, string> stationDict = new Dictionary<string, string>();
        public Dictionary<string, string> replaceToken = new Dictionary<string, string>();
        public Dictionary<string, string> trainDict = new Dictionary<string, string>();

        public Dictionary<string, Station> StationMap = new Dictionary<string, Station>();
        public Dictionary<string, TrainInfo> TrainMap = new Dictionary<string, TrainInfo>(); 

        public List<string> coachList = new List<string>
        {
            "none",
            "1 ac",
            "2 ac",
            "3 ac",
            "sleeper",
            "chair car",
            "general"
        };

        public List<string> berthList = new List<string>
        {
            "side upper",
            "side lower",
            "lower",
            "middle",
            "upper",
            "window",
            "no choice"
        };

        public Dictionary<string, CoachType> coachMap = new Dictionary<string, CoachType>
        {
            {"1 ac", CoachType.FirstAC},
            {"2 ac", CoachType.SecondAC},
            {"3 ac", CoachType.ThirdAC},
            {"1ac", CoachType.FirstAC},
            {"2ac", CoachType.SecondAC},
            {"3ac", CoachType.ThirdAC},
            {"sleeper", CoachType.Sleeper},
            {"seater", CoachType.Seeter},
            {"chair car", CoachType.ChairCar},
            {"general", CoachType.General}
        };

        private Dictionary<string, Gender> genderMap = new Dictionary<string, Gender>
        {
            {"m", Gender.Male},
            {"male", Gender.Male},
            {"man", Gender.Male},
            {"woman", Gender.Female},
            {"female", Gender.Female},
            {"f", Gender.Female}
        };

        private Dictionary<string, BookedState> bookedMap = new Dictionary<string, BookedState>
        {
            { "yes", BookedState.Confirmed},
            { "y", BookedState.Confirmed},
            { "no", BookedState.Cancelled},
            { "n", BookedState.Cancelled},
            { "not now", BookedState.Cancelled}
        }; 

        private readonly Dictionary<string, BerthChoice> berthMap = new Dictionary<string, BerthChoice>
        {
            {"su", BerthChoice.SideUpper},
            {"sl", BerthChoice.SideLower},
            {"l", BerthChoice.Lower},
            {"u", BerthChoice.Upper},
            {"m", BerthChoice.Middle},
            {"w", BerthChoice.Window},
            {"ws", BerthChoice.Window},
            {"n", BerthChoice.NoChoice},
            {"side upper", BerthChoice.SideUpper},
            {"side lower", BerthChoice.SideLower},
            {"lower", BerthChoice.Lower},
            {"upper", BerthChoice.Upper},
            {"middle", BerthChoice.Middle},
            {"window", BerthChoice.Window},
            {"no choice", BerthChoice.NoChoice}
        };

        public void Initialize()
        {
            StationMap = ConvertStationTsvToStationDictionary(@"TaskHackathon.Station_StationCode_StationName.tsv");
            replaceToken = LoadFileToDictionary(@"TaskHackathon.texteditor.txt");
            TrainMap = ConvertTrainTsvToTrainDictionary(@"TaskHackathon.Train_TrainNumber_TrainName.tsv");
        }

        //file contians 3 words
        public Dictionary<string, TrainInfo> ConvertTrainTsvToTrainDictionary(String tsvFilePath)
        {
            StreamReader reader = ReadFile(tsvFilePath);
            string readLine;
            while ((readLine = reader.ReadLine()) != null)
            {
                string[] a = readLine.Split('\t');
                TrainInfo trainInfo = new TrainInfo(a[2], a[1]);
                TrainMap[a[0]] = trainInfo;
            }
            reader.Close();
            return TrainMap;
        }

        //file contians 3 words
        public Dictionary<string, Station> ConvertStationTsvToStationDictionary(String tsvFilePath)
        {
            StreamReader reader = ReadFile(tsvFilePath);
            string readLine;
            while ((readLine = reader.ReadLine()) != null)
            {
                string[] a = readLine.Split('\t');
               Station station = new Station(a[2],a[1]);
               StationMap[a[0]] = station;
            }
            reader.Close();
            return StationMap;
        }


        public Classifier()
        {
            Initialize();
        }

        public bool TryUpdateTaskState(TaskAnswer answer, UserProfile userProfile, string query)
        {
            TrainBookingState taskState = answer.TaskState as TrainBookingState;
            if (taskState == null)
            {
                return false;
            }

            TaskStateCode currentState = answer.CurrentState;
            bool updated = false;
            switch (currentState)
            {
                case TaskStateCode.SourceStationState:
                    Station sourceStation;
                    if (TryGetStation(query, out sourceStation))
                    {
                        taskState.Source = sourceStation;
                        updated = true;
                    }
                    break;

                case TaskStateCode.DestinationStationState:
                    Station destinationStation;
                    if (TryGetStation(query, out destinationStation))
                    {
                        taskState.Destination = destinationStation;
                        updated = true;
                    }
                    break;

                case TaskStateCode.DateOfJourneyState:
                    DateTime date;
                    if (DateTime.TryParse(query, out date))
                    {
                        taskState.DateOfJourney = date;
                        updated = true;
                    }
                    break;

                case TaskStateCode.TrainSelectionState:
                    TrainInfo trainInfo;
                    if (TryGetTrain(query, out trainInfo))
                    {
                        taskState.TrainInfo = trainInfo;
                        updated = true;
                    }
                    break;

                case TaskStateCode.PreferredCoachState:
                    CoachType coachType;
                    if (TryGetCoach(query, out coachType))
                    {
                        taskState.CoachPreference = coachType;
                        updated = true;
                    }
                    break;

                case TaskStateCode.NumberOfPassangersState:
                    uint number;
                    if (TryGetNumberOfPassengers(query, out number))
                    {
                        taskState.NumberOfPassangers = number;
                        updated = true;
                    }
                    break;

                case TaskStateCode.PassengerNameState:
                    string name;
                    if (TryGetPassengerName(query, out name))
                    {
                        uint iPassenger = answer.CurrentPassengerId;
                        var passengerList = taskState.PassangerInfoList;
                        var passenger = passengerList.FirstOrDefault(x => x.Id == iPassenger);
                        if (passenger == null)
                        {
                            passenger = new PassangerInfo();
                            passengerList.Add(passenger);
                            passenger.Id = (uint)passengerList.Count;
                            answer.CurrentPassengerId = passenger.Id;
                        }
                        passenger.Name = name;
                        updated = true;
                    }
                    break;

                case TaskStateCode.PassengerAgeState:
                    uint age;
                    if (TryGetPassengerAge(query, out age))
                    {
                        uint iPassenger = answer.CurrentPassengerId;
                        var passengerList = taskState.PassangerInfoList;
                        var passenger = passengerList.FirstOrDefault(x => x.Id == iPassenger);
                        if (passenger == null)
                        {
                            passenger = new PassangerInfo();
                            passengerList.Add(passenger);
                            passenger.Id = (uint)passengerList.Count;
                            answer.CurrentPassengerId = passenger.Id;
                        }
                        passenger.Age = age;
                        updated = true;
                    }
                    break;

                case TaskStateCode.PassengerGenderState:
                    Gender gender;
                    if (TryGetPassengerGender(query, out gender))
                    {
                        uint iPassenger = answer.CurrentPassengerId;
                        var passengerList = taskState.PassangerInfoList;
                        var passenger = passengerList.FirstOrDefault(x => x.Id == iPassenger);
                        if (passenger == null)
                        {
                            passenger = new PassangerInfo();
                            passengerList.Add(passenger);
                            passenger.Id = (uint)passengerList.Count;
                            answer.CurrentPassengerId = passenger.Id;
                        }
                        passenger.Gender = gender;
                        updated = true;
                    }
                    break;

                case TaskStateCode.PassengerSeatPreferenceState:
                    BerthChoice berthChoice;
                    if (TryGetPassengerSeatPreference(query, out berthChoice))
                    {
                        uint iPassenger = answer.CurrentPassengerId;
                        var passengerList = taskState.PassangerInfoList;
                        var passenger = passengerList.FirstOrDefault(x => x.Id == iPassenger);
                        if (passenger == null)
                        {
                            passenger = new PassangerInfo();
                            passengerList.Add(passenger);
                            passenger.Id = (uint)passengerList.Count;
                            answer.CurrentPassengerId = passenger.Id;
                        }
                        passenger.BerthChoice = berthChoice;
                        updated = true;
                    }
                    break;

                case TaskStateCode.PhoneNumberState:
                    string phoneNumber;
                    if (TryGetPhoneNumber(query, out phoneNumber))
                    {
                        taskState.PhoneNumber = phoneNumber;
                        updated = true;
                    }
                    break;

                case TaskStateCode.BookTicketState:
                    BookedState ticketBookedState;
                    if (TryGetConfirmation(query, out ticketBookedState))
                    {
                        answer.TicketBookedState = ticketBookedState;
                        updated = true;
                    }
                    break;
            }

            return updated;

        }

        private bool TryGetConfirmation(string query, out BookedState bookedState)
        {
            bool retunStatus = false;
            string genderString = CleanInput(query.ToLower());
            if (bookedMap.TryGetValue(genderString, out bookedState))
            {
                retunStatus = true;
            }
            return retunStatus;
        }



        public bool TryGetStation(string query, out Station sourceStation)
        {
            string station = CleanInput(query.ToLower());
            return StationMap.TryGetValue(station, out sourceStation);
        }


        public bool TryGetStationX(string query, out Station sourceStation)
        {
            sourceStation = null;
            bool validEntity = false;
            string station = CleanInput(query.ToLower());
            validEntity = IsValidEntity(stationDict, station);
            if (validEntity)
            {
                sourceStation = new Station();
                sourceStation.Name = station;
                sourceStation.Code = stationDict[station];
            }
            return validEntity;
        }

        public bool TryGetTrain(string query, out TrainInfo trainInfo)
        {
            trainInfo = null;
            bool validEntity = false;
            string trainCode = CleanInputTextToNumbers(query.ToLower());
            return TrainMap.TryGetValue(trainCode, out trainInfo);
        }

        public bool TryGetTrainX(string query, out TrainInfo trainInfo)
        {
            trainInfo = null;
            bool validEntity = false;
            string trainCode = CleanInputTextToNumbers(query.ToLower());
            validEntity = IsValidEntity(trainDict, trainCode);
            if (validEntity)
            {
                trainInfo = new TrainInfo();
                trainInfo.Name = trainDict[trainCode];
                trainInfo.Number = trainCode;
            }
            return validEntity;
        }

        public bool TryGetCoach(string query, out CoachType coachType)
        {
            string coachString = CleanInput(query.ToLower());
            return coachMap.TryGetValue(coachString, out coachType);
        }

        public bool TryGetPassengerAge(string query, out uint age)
        {
            bool retunStatus = false;
            string ageString = CleanInputTextToNumbers(query.ToLower());
            if (UInt32.TryParse(ageString, out age))
            {
                retunStatus = true;
            }
            return retunStatus;
        }

        public bool TryGetPassengerGender(string query, out Gender gender)
        {
            bool retunStatus = false;
            string genderString = CleanInput(query.ToLower());
            if (genderMap.TryGetValue(genderString, out gender))
            {
                retunStatus = true;
            }
            return retunStatus;
        }

        public bool TryGetPassengerSeatPreference(string query, out BerthChoice berthChoice)
        {
            bool retunStatus = false;
            string berthChoiceString = CleanInput(query.ToLower());
            if (berthMap.TryGetValue(berthChoiceString, out berthChoice))
            {
                retunStatus = true;
            }
            return retunStatus;
        }

        public bool TryGetPhoneNumber(string query, out string phoneNumber)
        {
            uint number;
            bool retunStatus = false;
            phoneNumber = null;
            string phoneNumberString = CleanInputTextToNumbers(query.ToLower());
            if (UInt32.TryParse(phoneNumberString, out number))
            {
                phoneNumber = phoneNumberString;
                retunStatus = true;
            }
            return retunStatus;
        }

        public bool TryGetNumberOfPassengers(string query, out uint number)
        {
            if (UInt32.TryParse(query.ToLower(), out number))
            {
                if (number <= 6)
                {
                    return true;
                }
            }
            return false;
        }

        public bool TryGetPassengerName(string query, out string name)
        {
            name = CleanInput(query.ToLower());
            return true;
        }

        private static string CleanInputTextToNumbers(string p)
        {
            Regex regex = new Regex(@"[0-9]+$");
            MatchCollection matches = regex.Matches(p);
            foreach (Match m in matches)
            {
                if (m.Success)
                {
                    return matches[0].Value; 
                }
            }
            return null;
        }

        private static bool IsValidEntity(Dictionary<string, string> dict, string value)
        {
            if (value != null && dict.ContainsKey(value.ToLower()))
                return true;
            return false;
        }

        private string CleanInput(string text)
        {
            string[] entity = text.Split(' ');
            StringBuilder cleanstring = new StringBuilder();
            foreach (string e in entity)
            {
                if (replaceToken.ContainsKey(e))
                    cleanstring.Append(e.Replace(e, replaceToken[e])).Append(' ');
                else
                    cleanstring.Append(e).Append(' ');
            }

            return cleanstring.ToString().Trim();
        }



        private static Dictionary<string, string> LoadFileToDictionary(string p)
        {
            StreamReader reader = ReadFile(p);
            Dictionary<string, string> dict = new Dictionary<string, string>();
            string readLine;
            while ((readLine = reader.ReadLine()) != null)
            {
                string[] a = readLine.Split('\t');
                if (!dict.ContainsKey(a[0].ToLower()))
                    dict.Add(a[0].ToLower(), String.Empty);
            }
            reader.Close();
            return dict;
            throw new System.NotImplementedException();
        }

        private static StreamReader ReadFile(string filePath)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = filePath;
            Stream stream = assembly.GetManifestResourceStream(resourceName);
            StreamReader reader = new StreamReader(stream);
            return reader;
        }

        private static List<string> LoadFileToList(string p)
        {
            StreamReader reader = new StreamReader(p);
            List<string> list = new List<string>();
            string readLine;
            while ((readLine = reader.ReadLine()) != null)
            {
                list.Add(readLine);
            }
            reader.Close();
            return list;
            throw new System.NotImplementedException();
        }
    }
}
   