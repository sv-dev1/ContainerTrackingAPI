using System;
using System.Collections.Generic;

namespace ContainerTrackingWebApi.Models
{
    public partial class SheetRowEmail
    {
        public int Id { get; set; }
        public int ContainerId { get; set; }
        public string SheetId { get; set; }
        public string RowId { get; set; }
        public string Email { get; set; }
    }
}
