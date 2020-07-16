using System;
using System.Collections.Generic;

namespace ContainerTrackingWebApi.Models
{
    public partial class ColumnsShow
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public short ShipmentRef { get; set; }
        public short Origin { get; set; }
        public short ContainerType { get; set; }
        public short Destination { get; set; }
        public short ContainerNo { get; set; }
        public short Departure { get; set; }
        public short Arrival { get; set; }
        public short FirstArrival { get; set; }
        public short ShippingLine { get; set; }
        public short Status { get; set; }
        public short? EarlyDelay { get; set; }
        public short FromCountry { get; set; }
        public short ToCountry { get; set; }
        public short TransitTime { get; set; }
        public short FirstEta { get; set; }
        public short BlReferenceNo { get; set; }
        public short TransitPorts { get; set; }
        public short GetoutDate { get; set; }
        public short EmptyReturnDate { get; set; }
        public short ShipmentBy { get; set; }
        public short? DaysBeforeArrival { get; set; }
        public short? Vesselname { get; set; }

        public virtual Users User { get; set; }
    }
}
