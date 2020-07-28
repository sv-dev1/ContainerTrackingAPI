using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContainerTrackingWebApi.ViewModel
{
    public class ContainerTrack
    {
        public int id { get; set; }
        public string name { get; set; }
        public string state { get; set; }
        public string country_code { get; set; }
        public string country { get; set; }
        public string locode { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
        public List<vessels> events { get; set; }
    }

    public class vessels
    {
        public int id { get; set; }
        public string name { get; set; }
        public string imo { get; set; }
        public string call_sign { get; set; }
        public string mmsi { get; set; }
        public string flag { get; set; }
    }
    public class events
    {
        public int location { get; set; }
        public string description { get; set; }
        public string status { get; set; }
        public string date { get; set; }
        public string actual { get; set; }
        public string type { get; set; }
        public int? vessel { get; set; }
        public string voyage { get; set; }
    }
    public class routeEvents
    {
        // public string name { get; set; }
        public List<routepol> routepol { get; set; }
    }
    public class routepol
    {
        public int location { get; set; }
        public string date { get; set; }
        public bool actual { get; set; }
    }



    public  class EventList
    {
        public long Location { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
        public bool Actual { get; set; }
        public string Type { get; set; }
        public long? Vessel { get; set; }
        public string Voyage { get; set; }
    }
}
