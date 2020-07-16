using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContainerTrackingWebApi.ViewModel
{
    public class ShipsgoContainerVM
    {
        public int id { get; set; }
        public string user_id { get; set; }
        public string po_no { get; set; }
        public string origin { get; set; }
        public string container_type { get; set; }
        public string destination { get; set; }
        public string container_no { get; set; }
        public DateTime departure { get; set; }
        public DateTime arrival { get; set; }
        public DateTime first_arrival { get; set; }
        public string shipping_line { get; set; }
        public int? is_deleted { get; set; }
        public string status { get; set; }
        public int shipsgo_id { get; set; }
        public string early_delay { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string eta { get; set; }
        public string from_country { get; set; }
        public string to_country { get; set; }
        public string transit_time { get; set; }
        public DateTime first_eta { get; set; }
        public string bl_reference_no { get; set; }
        public string transit_ports { get; set; }
        public DateTime getout_date { get; set; }
        public DateTime empty_return_date { get; set; }
        public int shipment_by { get; set; }
        public string days_before_arrival { get; set; }
    }
}
