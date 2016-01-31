using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModels.Answer;
using DataModels.Common;
using Newtonsoft.Json;

namespace TaskTest
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateSampleAnswer();
        }

        static void CreateSampleAnswer()
        {
            ChatItem item1 = new ChatItem();
            item1.Id = "Cortana";
            item1.ChatText = "Hi this is Cortana";

            ChatItem item2 = new ChatItem();
            item2.Id = "User";
            item2.ChatText = "Hi this is User";

            ChatItem item3 = new ChatItem();
            item3.Id = "Cortana";
            item3.ChatText = "Hi shall we complete the Task?";

            ChatWindow chatWindow = new ChatWindow();
            chatWindow.ChatList.Add(item1);
            chatWindow.ChatList.Add(item2);
            chatWindow.ChatList.Add(item3);

            TrainBookingState bookingState = new TrainBookingState();

            bookingState.Source = new Station();
            bookingState.Source.Name = "Hyderabad";
            bookingState.Source.Code = "SC";

            bookingState.Destination = new Station();
            bookingState.Destination.Name = "Chennai Central";
            bookingState.Destination.Code = "MAS";

            bookingState.DateOfJourney = DateTime.Now.AddDays(10);

            bookingState.TrainInfo = new TrainInfo();
            bookingState.TrainInfo.Name = "Chennai Express";
            bookingState.TrainInfo.Number = "12604";

            bookingState.CoachPreference = CoachType.SecondAC;
            bookingState.PhoneNumber = "1234567890";

            bookingState.NumberOfPassangers = 4;

            PassangerInfo pas1 = new PassangerInfo();
            pas1.Name = "BillGates";
            pas1.Age = 50;
            pas1.Gender = Gender.Male;
            pas1.BerthChoice = BerthChoice.SideLower;

            PassangerInfo pas2 = new PassangerInfo();
            pas2.Name = "Balmer";
            pas2.Age = 49;
            pas2.Gender = Gender.Male;
            pas2.BerthChoice = BerthChoice.SideUpper;

            PassangerInfo pas3 = new PassangerInfo();
            pas3.Name = "BillGates";
            pas3.Age = 45;
            pas3.Gender = Gender.Male;
            pas3.BerthChoice = BerthChoice.Lower;

            bookingState.PassangerInfoList = new List<PassangerInfo>();
            bookingState.PassangerInfoList.Add(pas1);
            bookingState.PassangerInfoList.Add(pas2);
            bookingState.PassangerInfoList.Add(pas3);


            TaskAnswer answer = new TaskAnswer();
            answer.Title = "Book Train Ticket Task";
            answer.TaskType = TaskType.Train;
            answer.ChatWindow = chatWindow;
            answer.TaskState = bookingState;


            string jsonString = JsonConvert.SerializeObject(answer);

            return;



        }
    }
}
