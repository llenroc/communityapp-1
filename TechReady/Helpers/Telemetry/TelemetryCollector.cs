using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;

namespace TechReady.Helpers.Telemetry
{
    class TelemetryCollector
    {
        private static TelemetryClient tc = new TelemetryClient();

        
        public static void ReportExepction(Exception ex)
        {
            tc.TrackException(ex);
        }
    }
}
