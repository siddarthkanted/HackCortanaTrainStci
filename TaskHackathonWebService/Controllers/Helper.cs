using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Routing;
using Microsoft.AspNet.Identity;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace TaskHackathonWebService.Controllers
{
    public static class Helper
    {
        public const string UnsupportedMode = "Unsupported Mode: {0}. Supported Modes are 'save' and 'publish'";
        public const string NotDeserializable = "Unable to Deserialize the Job Object.";
        public const string JSonMediaType = "application/json";

        public static HttpResponseMessage CreateHttpResponseMessage(HttpRequestMessage request, HttpStatusCode code,
            string content)
        {
            HttpResponseMessage response;

            response = request.CreateResponse(code);
            response.Content = new StringContent(content, Encoding.UTF8, JSonMediaType);
            response.Headers.Add("Access-Control-Allow-Origin", "*");

            return response;
        }

        public static HttpStatusCode GetStatusCode(string status, bool isCreateOperation = false)
        {
            return HttpStatusCode.OK;
            //if (status.Equals(Constants.INPROGRESS))
            //{
            //    return HttpStatusCode.Accepted;
            //}

            //if (status.Equals(Constants.SUCCESS))
            //{
            //    if (isCreateOperation)
            //    {
            //        return HttpStatusCode.Accepted;
            //    }
            //    return HttpStatusCode.OK;
            //}
            //return HttpStatusCode.BadRequest;
        }

        //public static bool AuthorizeUser(HttpRequestMessage request, out string userId)
        //{
        //    userId = string.Empty;
        //    IEnumerable<string> values;

        //    if (RoleEnvironment.IsEmulated)
        //    {
        //        userId = System.Security.Principal.WindowsIdentity.GetCurrent().Name + "@" +
        //                 Environment.MachineName;
        //    }
        //    else
        //    {
        //        if (request.Headers.TryGetValues(Constants.USER_ID, out values))
        //        {
        //            userId = values.FirstOrDefault();
        //        }
        //    }

        //    if (string.IsNullOrEmpty(userId))
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        //public static RequestContext CreateContext(HttpRequestMessage request, string userId, string api, string environment, string mode, string section, string jobName, string comment = null)
        //{
        //    string id = Guid.NewGuid().ToString();

        //    string clientId = GetClientName(request);

        //    string operation = GetOperation(request.Method);

        //    return new RequestContext(id, userId, clientId, api, operation, environment, mode, section, jobName, comment);
        //}

        //private static string GetOperation(HttpMethod method)
        //{
        //    string operation;
        //    if (method == HttpMethod.Get)
        //    {
        //        operation = Constants.READ;
        //    }
        //    else if (method == HttpMethod.Post)
        //    {
        //        operation = Constants.CREATE;
        //    }
        //    else if (method == HttpMethod.Put)
        //    {
        //        operation = Constants.UPDATE;
        //    }
        //    else if (method == HttpMethod.Delete)
        //    {
        //        operation = Constants.DELETE;
        //    }
        //    else
        //    {
        //        operation = method.ToString();
        //    }
        //    return operation;
        //}

        //private static string GetClientName(HttpRequestMessage request)
        //{
        //    string clientName = string.Empty;
        //    if (RoleEnvironment.IsEmulated)
        //    {
        //        clientName = "Local";
        //    }
        //    else
        //    {
        //        var certificate = request.GetClientCertificate();
        //        if (certificate != null && !string.IsNullOrEmpty(certificate.Subject))
        //        {
        //            clientName = certificate.Subject;
        //        }
        //    }
        //    return clientName;
        //}
    }
}