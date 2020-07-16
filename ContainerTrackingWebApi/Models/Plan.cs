using System;
using System.Collections.Generic;

namespace ContainerTrackingWebApi.Models
{
    public partial class Plan
    {
        public Plan()
        {
            Payment = new HashSet<Payment>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string MonthlyPrice { get; set; }
        public string YearlyPrice { get; set; }
        public string OrgYearlyPrice { get; set; }
        public string TrackingShipment { get; set; }
        public int YearlyTrackingShipment { get; set; }
        public float Vat { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<Payment> Payment { get; set; }
    }
}
