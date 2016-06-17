using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechReady.NavigationParameters
{
    public class EventsFilterPageNavigationParameters : NavigationParameter
    {
        public string Location { get; set; }
        public string Technology { get; set; }
        public string Role { get; set; }

        public EventsFilterPageNavigationParameters(string fromPageName)
        {
            this.FromPageName = fromPageName;
        }

    }

    public class LearningResourcesFilterPageNavigationParameters : NavigationParameter
    {
        public string Type { get; set; }
        public string Technology { get; set; }
        public string Role { get; set; }

        public LearningResourcesFilterPageNavigationParameters(string fromPageName)
        {
            this.FromPageName = fromPageName;
        }

    }
    public class ResetFilterPageNavigationParameters : NavigationParameter
    {
        public string Location { get; set; } = string.Empty;
        public string Technology { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;

        public ResetFilterPageNavigationParameters(string fromPageName)
        {
            this.FromPageName = fromPageName;
        }

    }
}
