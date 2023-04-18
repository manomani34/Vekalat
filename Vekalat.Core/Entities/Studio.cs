using Vekalat.Application.Common;
using System;
using System.Collections.Generic;

namespace Vekalat.Core.Entities
{
    public class Studio : AuditedEntity
    {
        public string Title { get; set; }
        public Guid UniqueCode { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Features { get; set; }
        public string Imagename { get; set; }
        public string Address { get; set; }
        public string Location { get; set; }
        public string Email { get; set; }
        public string Tell { get; set; }
        public bool IsActive { get; set; }

        public List<StudioGallery> StudioGalleries { get; set; }
        public List<StudioReservation> StudioReservations { get; set; }
        public List<StudioReservationHistory> StudioReservationHistories { get; set; }

    }
}
