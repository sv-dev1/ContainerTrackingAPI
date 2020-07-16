using System;
using System.Collections.Generic;

namespace ContainerTrackingWebApi.Models
{
    public partial class Payment
    {
        public Payment()
        {
            AvailableContainers = new HashSet<AvailableContainers>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public int PlanId { get; set; }
        public float SubAmount { get; set; }
        public float VatAmount { get; set; }
        public float TotalAmount { get; set; }
        public string TransactionId { get; set; }
        public string Status { get; set; }
        public string RowData { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime ExpiryDate { get; set; }

        public virtual Plan Plan { get; set; }
        public virtual Users User { get; set; }
        public virtual ICollection<AvailableContainers> AvailableContainers { get; set; }
    }
}
