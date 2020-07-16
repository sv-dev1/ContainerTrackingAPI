using System;
using System.Collections.Generic;

namespace ContainerTrackingWebApi.Models
{
    public partial class ShipsgoContainer
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string PoNo { get; set; }
        public string Origin { get; set; }
        public string ContainerType { get; set; }
        public string Destination { get; set; }
        public string ContainerNo { get; set; }
        public DateTime Departure { get; set; }
        public DateTime Arrival { get; set; }
        public DateTime FirstArrival { get; set; }
        public string ShippingLine { get; set; }
        public string Status { get; set; }
        public short IsDeleted { get; set; }
        public string ShipsgoId { get; set; }
        public string EarlyDelay { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Eta { get; set; }
        public string FromCountry { get; set; }
        public string ToCountry { get; set; }
        public string TransitTime { get; set; }
        public DateTime FirstEta { get; set; }
        public string BlReferenceNo { get; set; }
        public string TransitPorts { get; set; }
        public DateTime GetoutDate { get; set; }
        public DateTime EmptyReturnDate { get; set; }
        public string ShipmentBy { get; set; }
        public string DaysBeforeArrival { get; set; }
        public string CompanyName { get; set; }
        public string Type56 { get; set; }
        public string Latfinal { get; set; }
        public string Lngfinal { get; set; }
        public string ShippingLink { get; set; }
        public string Day { get; set; }
        public string Time { get; set; }

        public virtual Users User { get; set; }
    }
}
