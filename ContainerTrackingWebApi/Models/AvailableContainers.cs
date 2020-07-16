using System;
using System.Collections.Generic;

namespace ContainerTrackingWebApi.Models
{
    public partial class AvailableContainers
    {
        public int Id { get; set; }
        public int PaymentId { get; set; }
        public int Containers { get; set; }

        public virtual Payment Payment { get; set; }
    }
}
