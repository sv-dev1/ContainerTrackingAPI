using System;
using System.Collections.Generic;

namespace ContainerTrackingWebApi.Models
{
    public partial class ContainerApidata
    {
        public int Id { get; set; }
        public string ApiData { get; set; }
        public int? Userid { get; set; }
        public string Day { get; set; }
        public string Time { get; set; }
        public string ContainerNo { get; set; }
        public string Status { get; set; }
        public string EmailStatus { get; set; }

        public virtual Users User { get; set; }
    }
}
