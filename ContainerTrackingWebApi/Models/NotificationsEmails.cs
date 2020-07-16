using System;
using System.Collections.Generic;

namespace ContainerTrackingWebApi.Models
{
    public partial class NotificationsEmails
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ConAddEmails { get; set; }
        public string DepChangeEmails { get; set; }
        public string ArrChangeEmails { get; set; }
        public string ConDelEmails { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string ConTimeoutEmails { get; set; }
        public string UntilArrivalEmails { get; set; }

        public virtual Users User { get; set; }
    }
}
