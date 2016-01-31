using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModels.Common;

namespace DataModels.Answer
{
    public class TaskAnswer
    {
        public TaskAnswer()
        {
            TaskType = TaskType.Train;
            TaskState = new TrainBookingState();
            ChatWindow = new ChatWindow();
            CurrentState = TaskStateCode.InitState;
            NextState = TaskStateCode.InitState;
            CurrentPassengerId = 1;
            CompletedPassengers = 0;
            TicketBookedState = BookedState.None;
            IsStateFinished = false;
        }

        public string UserId;

        public string SessionId;

        public string Title;

        public TaskType TaskType;

        public ITaskState TaskState;

        public ChatWindow ChatWindow;

        public TaskStateCode CurrentState;

        public TaskStateCode NextState;

        public uint CurrentPassengerId;

        public uint CompletedPassengers;

        public uint IterationCount;

        public BookedState TicketBookedState;

        public bool IsStateFinished;
    }
}
