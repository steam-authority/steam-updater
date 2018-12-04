﻿using System;
using Google.Api;
using Google.Cloud.Logging.Type;
using Google.Cloud.Logging.V2;
using Rollbar;

namespace Updater
{
    public static class Log
    {
        private const string key = "STEAM_PROXY_ROLLBAR_PRIVATE";
        private const string proj = "STEAM_GOOGLE_PROJECT";
        private const string env = "STEAM_ENV";

        private static readonly LoggingServiceV2Client googleCLient = LoggingServiceV2Client.Create();

        public static void GoogleInfo(string message)
        {
            Console.WriteLine(message);

            var googleProject = Environment.GetEnvironmentVariable(proj);
            var environment = Environment.GetEnvironmentVariable(env);

            var logName = new LogName(googleProject, environment + "-updater");
            var resource = new MonitoredResource {Type = "project"};
            var logEntry = new LogEntry
            {
                LogName = logName.ToString(),
                Severity = LogSeverity.Info,
                TextPayload = message
            };

            googleCLient.WriteLogEntries(LogNameOneof.From(logName), resource, null, new[] {logEntry});
        }

        public static void setupRollbar()
        {
            var rollbarKey = Environment.GetEnvironmentVariable(key);
            var environment = Environment.GetEnvironmentVariable(env);

            if (!string.IsNullOrEmpty(rollbarKey))
            {
                Console.WriteLine("Rollbar key environment variable not found");

                var config = new RollbarConfig(rollbarKey)
                {
                    Environment = environment
                };

                RollbarLocator.RollbarInstance.Configure(config);
            }
        }

        public static void RollbarError(string message)
        {
            RollbarLocator.RollbarInstance.Error(message);
        }

        public static void RollbarInfo(string message)
        {
            RollbarLocator.RollbarInstance.Info(message);
        }
    }
}