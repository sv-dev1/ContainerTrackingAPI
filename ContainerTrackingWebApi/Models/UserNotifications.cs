using System;
using System.Collections.Generic;

namespace ContainerTrackingWebApi.Models
{
    public partial class UserNotifications
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public short ConAddSts { get; set; }
        public short ConAddTime { get; set; }
        public short DepChangeSts { get; set; }
        public short DepChangeTime { get; set; }
        public short ArrChangeSts { get; set; }
        public short ArrChangeTime { get; set; }
        public short ConDelSts { get; set; }
        public short ConDelTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public short? ConTimeoutSts { get; set; }
        public short? ConTimeoutTime { get; set; }
        public short? ConTimeoutSendsts { get; set; }
        public short? ConUntilarrivalByEmail { get; set; }
        public short? ConUntilarrivalDays { get; set; }

        public virtual Users User { get; set; }
    }
}
