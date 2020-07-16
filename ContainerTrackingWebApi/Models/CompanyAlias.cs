using System;
using System.Collections.Generic;

namespace ContainerTrackingWebApi.Models
{
    public partial class CompanyAlias
    {
        public int Id { get; set; }
        public int? Userid { get; set; }
        public string Name { get; set; }
        public int? IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? ModifiedBy { get; set; }

        public virtual Users User { get; set; }
    }
}
