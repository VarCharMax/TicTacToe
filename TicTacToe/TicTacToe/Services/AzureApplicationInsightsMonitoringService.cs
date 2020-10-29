﻿using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using System;
using System.Collections.Generic;

namespace TicTacToe.Services
{
    public class AzureApplicationInsightsMonitoringService : IMonitoringService
    {
        readonly TelemetryClient _telemetryClient = new TelemetryClient();

        public void TrackEvent(string eventName, TimeSpan elapsed, IDictionary<string, string> properties = null)
        {
            var telemetry = new EventTelemetry(eventName);

            telemetry.Metrics.Add(eventName, elapsed.TotalMilliseconds);

            if (properties != null)
            {
                foreach (var property in properties)
                {
                    telemetry.Properties.Add(property.Key, property.Value);
                }
                _telemetryClient.TrackEvent(telemetry);
            }
        }
    }
}
