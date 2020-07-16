using System;
using System.Collections.Generic;

namespace ContainerTrackingWebApi.Models
{
    public partial class SheetData
    {
        public int Id { get; set; }
        public int ContainerId { get; set; }
        public string SheetId { get; set; }
        public string RowId { get; set; }
        public string PoNo { get; set; }
        public string Origin { get; set; }
        public string ContainerType { get; set; }
        public string Destination { get; set; }
        public string ContainerNumber { get; set; }
        public DateTime Departure { get; set; }
        public DateTime Arrival { get; set; }
        public string ShippingLine { get; set; }
        public string Status { get; set; }
        public string EarlyDelay { get; set; }
        public DateTime CreatedAt { get; set; }
        public string FromCountry { get; set; }
        public string ToCountry { get; set; }
        public string TransitTime { get; set; }
        public string FirstEta { get; set; }
        public string BlReferenceNo { get; set; }
        public string TransitPorts { get; set; }
        public DateTime GetoutDate { get; set; }
        public DateTime EmptyReturnDate { get; set; }
        public string ShipmentBy { get; set; }
    }
}
