using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using DataModels.Answer;
using DataModels.Common;
using DataModels.Helper;
using DataModels.Profile;
using Newtonsoft.Json;

namespace TaskHackathon
{
    public class TrainTaskStateMachine
    {
        private Dictionary<TaskStateCode, string> _chatItemMap;

        private StorageBlob blobStorage;

        private Classifier classifier;

        private UserProfileHelper profileHelper;

        private void InitializeChatItemMap()
        {
            _chatItemMap = new Dictionary<TaskStateCode, string>()
            {
                { TaskStateCode.SourceStationState, "Let me know the Source Location"},
                { TaskStateCode.DestinationStationState, "Let me know the Destination Location"},
                { TaskStateCode.DateOfJourneyState, "When are you planning to travel? Format(DD-MM-YYYY)"},
                { TaskStateCode.TrainSelectionState, "These are the following trains run between {0} to {1}. Choose any one"},
                { TaskStateCode.PreferredCoachState, "Please select the coach preference (1ac, 2ac, 3ac, sl, sleeper etc)" },
                { TaskStateCode.NumberOfPassangersState, "How many are travelling? (number below 5, just enter 1 or 2)"},
                { TaskStateCode.PassengerNameState, "Let me know the passenger {0} details. What is the name?"},
                { TaskStateCode.PassengerGenderState, "Let me know the {0}'s Gender."},
                { TaskStateCode.PassengerAgeState, "What is {0} Age? (number)"},
                { TaskStateCode.PassengerSeatPreferenceState, "Select the seat preference (sl,su,l,m,u etc.)"},
                { TaskStateCode.PhoneNumberState, "Let me know the phone number (number)"},
                { TaskStateCode.PassengerRelationState, "Please select the relation, next time when we book, I can autofill the details."},
                { TaskStateCode.BookTicketState, "These are the following details, Shall I go and book? (yes or no)"},
                { TaskStateCode.SaveState, "No problem, I can save this state for some time, you can ask me get the unfinished task details."},
                { TaskStateCode.EndState, "Thanks for Booking. We will show proactive cards, Something like, Book a Cab? Book a Hotel? Set a Remider? etc. :)"}
            };
        }

        public TrainTaskStateMachine(string connectionString, string containterName)
        {
            InitializeChatItemMap();

            blobStorage = new StorageBlob(connectionString, containterName);

            classifier = new Classifier();

            profileHelper = new UserProfileHelper();
        }

        public ChatItem GetUserChatItem(string userId, string query)
        {
            ChatItem chatItem = new ChatItem();
            chatItem.Id = userId;
            chatItem.ChatText = query;
            return chatItem;
        }

        public TaskAnswer RunState(TaskAnswer answer, UserProfile profile, string query, string queryType)
        {
            answer = PreStateAction(answer, profile, query);

            TrainBookingState taskState = answer.TaskState as TrainBookingState;
            if (taskState == null)
            {
                return answer;
            }
            TaskStateCode state = answer.CurrentState;

            bool isUpdated = false;
            answer.IterationCount++;

            switch (state)
            {
                case TaskStateCode.InitState:
                    answer.CurrentState = TaskStateCode.InitState;
                    answer.NextState = TaskStateCode.SourceStationState;
                    break;
                case TaskStateCode.SourceStationState:
                case TaskStateCode.DestinationStationState:
                    isUpdated = classifier.TryUpdateTaskState(answer, profile, query);
                    break;
                case TaskStateCode.DateOfJourneyState:
                    taskState.DateOfJourney = DateTime.ParseExact(query, "dd-MM-yyyy", null);
                    break;
                case TaskStateCode.TrainSelectionState:
                case TaskStateCode.PreferredCoachState:
                case TaskStateCode.NumberOfPassangersState:
                case TaskStateCode.PassengerNameState:
                case TaskStateCode.PassengerAgeState:
                case TaskStateCode.PassengerGenderState:
                case TaskStateCode.PassengerSeatPreferenceState:
                case TaskStateCode.PassengerDetailsState:
                case TaskStateCode.PhoneNumberState:
                case TaskStateCode.BookTicketState:
                    isUpdated = classifier.TryUpdateTaskState(answer, profile, query);
                    break;
                default:
                    break;
            }

            if (isUpdated)
            {
                answer.IterationCount = 0;
            }
            if (answer.IterationCount > 3)
            {
                DummyUpdateAnswer(answer, profile, query);
            }

            return PostStateAction(answer, profile, query);
        }


        private TaskAnswer PreStateAction(TaskAnswer answer, UserProfile profile, string query)
        {
            // Add the User Query to Chat List Window
            var userChatItem = GetUserChatItem(answer.UserId, query);
            answer.ChatWindow.ChatList.Add(userChatItem);

            return answer;
        }
        
        private TaskAnswer PostStateAction(TaskAnswer answer, UserProfile profile, string query)
        {
            TrainBookingState taskState = answer.TaskState as TrainBookingState;
            if (taskState == null)
            {
                return null;
            }

            TaskStateCode nextState = GetNextState(answer);
            answer.CurrentState = nextState;

            string question;
            if (!_chatItemMap.TryGetValue(nextState, out question))
            {
                question = "Some Error. it shouldn't";
                return null;
            }
            ChatItem chatItem = new ChatItem();
            PassangerInfo passenger = taskState.PassangerInfoList.FirstOrDefault(x => x.Id == answer.CurrentPassengerId);
            if (passenger == null)
            {
                // Fixme; avoid dummy passenger
                PassangerInfo passanger = new PassangerInfo();
            }
            
            switch (nextState)
            {
                case TaskStateCode.InitState:
                    break;
                case TaskStateCode.SourceStationState:
                    break;
                case TaskStateCode.DestinationStationState:
                    break;
                case TaskStateCode.DateOfJourneyState:
                    break;
                case TaskStateCode.TrainSelectionState:
                    question = string.Format(question, taskState.Source.Code, taskState.Destination.Code);
                    FillTrainList(chatItem, taskState.Source.Code, taskState.Destination.Code);
                    break;
                case TaskStateCode.PreferredCoachState:
                    FillPreferredCoachList(chatItem);
                    break;
                case TaskStateCode.NumberOfPassangersState:
                    break;
                case TaskStateCode.PassengerNameState:
                    question = string.Format(question, answer.CurrentPassengerId);
                    break;
                case TaskStateCode.PassengerGenderState:
                    question = string.Format(question, passenger.Name);
                    break;
                case TaskStateCode.PassengerAgeState:
                    question = string.Format(question, (passenger.Gender.Equals(Gender.Male)) ? "his" : "her");
                    break;
                case TaskStateCode.PassengerSeatPreferenceState:
                    FillePreferredBerthList(chatItem);
                    break;
                case TaskStateCode.PassengerDetailsState:
                    break;
                case TaskStateCode.PhoneNumberState:
                    break;
                case TaskStateCode.BookTicketState:
                    break;
                case TaskStateCode.EndState:
                    profileHelper.UpdateUserProfile(taskState, profile);
                    break;
                default:
                    break;
            }

            chatItem.Id = "Cortana";
            chatItem.ChatText = question;
            answer.ChatWindow.ChatList.Add(chatItem);
            return answer;
        }

        private void FillTrainList(ChatItem chatItem, string source, string destination)
        {
            List<Train> trainDataList = TrainUtil.DownlondJsonFromUrl(source, destination);
            List<string> stringList = TrainUtil.ConvertTrainDataListToStringList(trainDataList);
           
            chatItem.ChatList = stringList;
            chatItem.ListType = ChatListType.Row;
        }

        private void FillPreferredCoachList(ChatItem chatItem)
        {
            List<string> chatList = new List<string>();
            chatList.Add("1 AC");
            chatList.Add("2 AC");
            chatList.Add("3 AC");
            chatList.Add("SL");

            chatItem.ChatList = chatList;
            chatItem.ListType = ChatListType.Column;
        }

        private void FillePreferredBerthList(ChatItem chatItem)
        {
            List<string> chatList = new List<string>();
            chatList.Add("SU");
            chatList.Add("SL");
            chatList.Add("L");
            chatList.Add("M");
            chatList.Add("U");
            chatList.Add("NC");

            chatItem.ChatList = chatList;
            chatItem.ListType = ChatListType.Column;
        }

        private bool TryGetNextPassengerState(PassangerInfo passanger, out TaskStateCode nextState)
        {
            nextState = TaskStateCode.ExceptionState;
            if (string.IsNullOrEmpty(passanger.Name))
            {
                nextState = TaskStateCode.PassengerNameState;
                return true;
            }

            if (passanger.Gender.Equals(Gender.None))
            {
                nextState = TaskStateCode.PassengerGenderState;
                return true;
            }

            if (passanger.Age == 0)
            {
                nextState = TaskStateCode.PassengerAgeState;
                return true;
            }

            if (passanger.BerthChoice.Equals(BerthChoice.None))
            {
                nextState = TaskStateCode.PassengerSeatPreferenceState;
                return true;
            }
            return false;
        }

        private TaskStateCode GetNextState(TaskAnswer answer)
        {
            if (answer.TaskType.Equals(TaskType.Train))
            {
                var trainBookingState = answer.TaskState as TrainBookingState;

                if (trainBookingState.Source == null)
                {
                    return TaskStateCode.SourceStationState;
                }

                if (trainBookingState.Destination == null)
                {
                    return TaskStateCode.DestinationStationState;
                }

                if (trainBookingState.DateOfJourney == null)
                {
                    return TaskStateCode.DateOfJourneyState;
                }

                if (trainBookingState.TrainInfo == null)
                {
                    return TaskStateCode.TrainSelectionState;
                }

                if (trainBookingState.CoachPreference.Equals(CoachType.None))
                {
                    return TaskStateCode.PreferredCoachState;
                }

                if (trainBookingState.NumberOfPassangers == 0)
                {
                    return TaskStateCode.NumberOfPassangersState;
                }

                uint iPassenger = answer.CurrentPassengerId;
                var passenger = trainBookingState.PassangerInfoList.FirstOrDefault(x => x.Id == iPassenger);

                if (passenger == null)
                {
                    return TaskStateCode.PassengerNameState;
                }

                TaskStateCode nextState;
                if (TryGetNextPassengerState(passenger, out nextState))
                {
                    return nextState;
                }
                passenger.IsComplete = true;
                // FIXME: small hack, some how, how can avoid this.
                if (answer.CompletedPassengers < trainBookingState.NumberOfPassangers)
                {
                    answer.CompletedPassengers += 1;
                }

                if (answer.CompletedPassengers < trainBookingState.NumberOfPassangers)
                {
                    answer.CurrentPassengerId++;
                    return TaskStateCode.PassengerNameState;
                }

                if (answer.IsStateFinished)
                {
                    switch (answer.TicketBookedState)
                    {
                        case BookedState.None:
                            return TaskStateCode.BookTicketState;
                            break;
                        case BookedState.Cancelled:
                            return TaskStateCode.SaveState;
                            break;
                        case BookedState.Confirmed:
                            return TaskStateCode.EndState;
                            break;
                    }
                }

                if (answer.CompletedPassengers == trainBookingState.NumberOfPassangers)
                {
                    if (string.IsNullOrEmpty(trainBookingState.PhoneNumber))
                    {
                        return TaskStateCode.PhoneNumberState;
                    }

                    answer.IsStateFinished = true;
                    return TaskStateCode.BookTicketState;
                }
                
                if (answer.CurrentState == TaskStateCode.SaveState)
                {
                    return TaskStateCode.BookTicketState;
                }
                
                return TaskStateCode.ExceptionState;
            }
            return TaskStateCode.ExceptionState;
        }

        private ChatItem GetChatItem(TaskAnswer answer, TaskStateCode stateCode)
        {
            TrainBookingState taskState = answer.TaskState as TrainBookingState;
            if (taskState == null)
            {
                return null;
            }

            string question;
            if (!_chatItemMap.TryGetValue(stateCode, out question))
            {
                question = "Some Error. it shouldn't";
            }

            ChatItem item = new ChatItem();
            item.Id = "Cortana";
            item.ChatText = question;
            return item;
        }

        private string GetBlobKey(string userId, string sessionId)
        {
            return string.Concat(userId, "_", sessionId);
        }

        public string RunAnswer(string userId, string sessionId, string query, string queryType)
        {
            //  Get the Answer State from Storage
            bool isNewSession = false;
            TaskAnswer answer;
            if (string.IsNullOrEmpty(sessionId))
            {
                sessionId = Guid.NewGuid().ToString();
                answer = new TaskAnswer();
                answer.Title = "Train Ticket Booking";
                answer.UserId = userId;
                answer.SessionId = sessionId;
            }
            else
            {
                string content = blobStorage.DownloadFileAsText(GetBlobKey(userId, sessionId));
                if (string.IsNullOrEmpty(content))
                {
                    return null;
                }
                answer = JsonConvert.DeserializeObject<TaskAnswer>(content);
            }

            // Get the Profile State from Storage
            string profileString = blobStorage.DownloadFileAsText(userId);
            UserProfile userProfile = null;
            if (string.IsNullOrEmpty(profileString))
            {
                userProfile = new UserProfile();
                userProfile.UserId = userId;
                userProfile.MyProfile = new Person();
                userProfile.MyProfile.Name = userId;
            }
            
            // TODO: Classify the Query
            //answer = DummyUpdateAnswer(query, queryType, answer);
            // TODO: Run the state machine on Classifier Output
            
            // Update Profile
            answer = RunState(answer, userProfile, query, queryType);
           
                        
            // Store the Profile
            string userProfileString = JsonConvert.SerializeObject(userProfile);
            blobStorage.UploadFile(userId, userProfileString);

            // Store the State
            string stateString = JsonConvert.SerializeObject(answer);
            blobStorage.UploadFile(GetBlobKey(userId, sessionId), stateString);

            return stateString;
        }

        private TaskAnswer DummyUpdateAnswer(TaskAnswer answer, UserProfile profile, string query)
        {
            var state = answer.TaskState as TrainBookingState;

            switch (answer.CurrentState)
            {
                case TaskStateCode.InitState:
                    answer.CurrentState = TaskStateCode.InitState;
                    break;
                case TaskStateCode.SourceStationState:
                    state.Source = new Station();
                    state.Source.Name = "Hyderabad";
                    state.Source.Code = "SC";
                    break;
                case TaskStateCode.DestinationStationState:
                    state.Destination = new Station();
                    state.Destination.Name = "Hyderabad";
                    state.Destination.Code = "SC";
                    break;
                case TaskStateCode.DateOfJourneyState:
                    state.DateOfJourney = DateTime.Now.AddDays(5);
                    break;
                case TaskStateCode.TrainSelectionState:
                    state.TrainInfo = new TrainInfo();
                    state.TrainInfo.Name = "Naryanadri";
                    state.TrainInfo.Number = "123456";
                    break;
                case TaskStateCode.PreferredCoachState:
                    state.CoachPreference = CoachType.FirstAC;
                    break;
                case TaskStateCode.NumberOfPassangersState:
                    state.NumberOfPassangers = 1;
                    break;
                case TaskStateCode.PassengerNameState:
                    break;
                case TaskStateCode.PassengerAgeState:
                    break;
                case TaskStateCode.PassengerGenderState:
                    break;
                case TaskStateCode.PassengerSeatPreferenceState:
                    break;
                case TaskStateCode.PassengerDetailsState:
                    break;
                case TaskStateCode.PhoneNumberState:
                    break;
                case TaskStateCode.BookTicketState:
                    break;
                default:
                    break;
            }

            return answer;
        }


    }
}
