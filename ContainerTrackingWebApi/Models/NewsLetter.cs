using System;
using System.Collections.Generic;

namespace ContainerTrackingWebApi.Models
{
    public partial class NewsLetter
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public short Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
