using System;
using System.Collections.Generic;

namespace ContainerTrackingWebApi.Models
{
    public partial class EmailLogs
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ContainerId { get; set; }
        public string Email { get; set; }
        public short ArrivalMail { get; set; }
        public short DepartureMail { get; set; }
        public short ConAddMail { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Users User { get; set; }
    }
}
