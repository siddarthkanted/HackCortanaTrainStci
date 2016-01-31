using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using TaskHackathon;

namespace TaskHackathonWebService.Controllers
{
    public class TaskController : ApiController
    {
        // GET: api/Task
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        public HttpResponseMessage Get(string userId, string query, string queryType, string sessionId = null)
        {
            var stateMachine = TaskHackathonEnvironment.GetStateMachine();

            var answerString = stateMachine.RunAnswer(userId, sessionId, query, queryType);
            if (string.IsNullOrEmpty(answerString))
            {
                return Helper.CreateHttpResponseMessage(Request, HttpStatusCode.BadRequest, "");
            }
            return Helper.CreateHttpResponseMessage(Request, HttpStatusCode.OK, answerString);
        }


        // GET: api/Task/5
        public HttpResponseMessage Get(int id)
        {
            string jsonString =
                "{\"Title\":\"Book Train Ticket Task\",\"TaskType\":0,\"TaskState\":{\"Source\":{\"Name\":\"Hyderabad\",\"Code\":\"SC\"},\"Destination\":{\"Name\":\"Chennai Central\",\"Code\":\"MAS\"},\"DateOfJourney\":\"2016-02-08T15:24:26.7730057+05:30\",\"TrainInfo\":{\"Name\":\"Chennai Express\",\"Number\":\"12604\"},\"CoachPreference\":1,\"NumberOfPassangers\":4,\"PassangerInfoList\":[{\"Name\":\"BillGates\",\"Age\":50,\"Gender\":0,\"BerthChoice\":1},{\"Name\":\"Balmer\",\"Age\":49,\"Gender\":0,\"BerthChoice\":0},{\"Name\":\"BillGates\",\"Age\":45,\"Gender\":0,\"BerthChoice\":2}],\"PhoneNumber\":\"1234567890\"},\"ChatWindow\":{\"ChatList\":[{\"Id\":\"Cortana\",\"ChatText\":\"Hi this is Cortana\",\"ChatList\":null},{\"Id\":\"User\",\"ChatText\":\"Hi this is User\",\"ChatList\":null},{\"Id\":\"Cortana\",\"ChatText\":\"Hi shall we complete the Task?\",\"ChatList\":null}]}}";

            return Helper.CreateHttpResponseMessage(Request, HttpStatusCode.OK, jsonString);
        }

        // POST: api/Task
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Task/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Task/5
        public void Delete(int id)
        {
        }
    }
}
