using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContainerTrackingWebApi.ViewModel
{
    public class SettingsVM
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public string userid { get; set; }
        public int? shipment_ref { get; set; }
        public int? origin { get; set; }
        public int? container_type { get; set; }
        public int? destination { get; set; }
        public int? container_no { get; set; }
        public int? departure { get; set; }
        public int? arrival { get; set; }
        public int? first_arrival { get; set; }
        public int? shipping_line { get; set; }
        public int? status { get; set; }
        public int? early_delay { get; set; }
        public int? from_country { get; set; }
        public int? to_country { get; set; }
        public int? transit_time { get; set; }
        public int? first_eta { get; set; }
        public int? bl_reference_no { get; set; }
        public int? transit_ports { get; set; }
        public int? getout_date { get; set; }
        public int? empty_return_date { get; set; }
        public int? shipment_by { get; set; }
        public int? days_before_arrival { get; set; }
        public int? vessel_name { get; set; }
    }
}
