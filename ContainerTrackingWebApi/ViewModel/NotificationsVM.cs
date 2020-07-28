using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContainerTrackingWebApi.ViewModel
{
    public class NotificationsVM
    {
        public int Id { get; set; }
        public string user_id { get; set; }
        public int ConAddSts { get; set; }
        public int ConAddTime { get; set; }
        public int DepChangeSts { get; set; }
        public int DepChangeTime { get; set; }
        public int ArrChangeSts { get; set; }
        public int ArrChangeTime { get; set; }
        public int ConDelSts { get; set; }
        public int ConDelTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int ConTimeoutSts { get; set; }
        public int ConTimeoutTime { get; set; }
        public int? ConTimeoutSendsts { get; set; }
        public long ConUntilarrivalByEmail { get; set; }
        public int ConUntilarrivalDays { get; set; }
        public string User { get; set; }

        public string con_add_emails { get; set; }//con_add_emails
        public string dep_change_emails { get; set; }
        public string arr_change_emails { get; set; }
        public string con_del_emails { get; set; }
        public string con_timeout_emails { get; set; }
        public string con_unit_emails { get; set; }
    }
}
