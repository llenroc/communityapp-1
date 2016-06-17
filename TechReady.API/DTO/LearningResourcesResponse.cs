using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechReady.Models;


namespace TechReady.Common.DTO
{
    public class LearningResourcesResponse
    {
        public ObservableCollection<LearningResource> LearningResources;
        public int CurrentPageNo { get; set; }
        public int TotalResults { get; set; }

    }
}
