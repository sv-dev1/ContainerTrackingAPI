using System;
using System.Collections.Generic;

namespace ContainerTrackingWebApi.Models
{
    public partial class CreateLog
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime LogoutTime { get; set; }
        public string LoginResult { get; set; }
        public string UserAgent { get; set; }
        public string SourceIp { get; set; }
    }
}
