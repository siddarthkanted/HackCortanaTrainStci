using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;

namespace TaskHackathon
{
    public static class TaskHackathonEnvironment
    {
        private static TrainTaskStateMachine StateMachine;
        public static void Initialize()
        {
            string connectionString = CloudConfigurationManager.GetSetting("STORAGE_CONNECTION_STRING");
            const string containerName = "taskcontainer";
            StateMachine = new TrainTaskStateMachine(connectionString, containerName);
        }

        public static TrainTaskStateMachine GetStateMachine()
        {
            return StateMachine;
        }
    }
}
