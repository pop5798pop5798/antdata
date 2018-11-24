using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Google.Cloud.Logging.Type;
using Google.Cloud.Logging.V2;
using Google.Api;

namespace SITAPI.Logging
{

    public class StackdriverLogger
    {
        private readonly LoggingServiceV2Client _client;
        private readonly LogNameOneof _logName;
        private readonly MonitoredResource _resource;

        public StackdriverLogger(string projectId,string logId)
        {
            _client = LoggingServiceV2Client.Create();
            _logName = LogNameOneof.From(new LogName(projectId, logId));
            _resource = new MonitoredResource { Type = "gce_backend_service" };
            
        }


    }
}