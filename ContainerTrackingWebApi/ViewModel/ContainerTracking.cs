using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContainerTrackingWebApi.ViewModel
{
    public class ContainerTracking
    {
        public string name { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
        public string country { get; set; }
        public List<containerEvents> events { get; set; }
    }
    public class containerEvents
    {
        public string date { get; set; }
        public string description { get; set; }
        public string vessel { get; set; }
        public string type;
    }
}
