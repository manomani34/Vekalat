using Vekalat.Core.Entities;
using Vekalat.Application.Common;
using Vekalat.Application.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vekalat.Core.Entities
{
    public class Equipment : AuditedEntity
    {
        public string Title { get; set; }
        public Guid UniqueCode { get; set; }
        public EquipmentStatus EquipmentStatus { get; set; }
        public EquipmentPhysicalStatus EquipmentPhysicalStatus { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Features { get; set; }
        public string Imagename { get; set; }
        public int Like { get; set; }
        public bool IsActive { get; set; }
        public int Quantity { get; set; }


        [ForeignKey("BrandId")]
        public Brand BrandFk { get; set; }

        [ForeignKey("CategoryId")]
        public Category CategoryFk { get; set; }

        public List<EquipmentGallery> EquipmentGalleries { get; set; }
        public List<EquipmentReservation> EquipmentReservations { get; set; }
        public List<EquipmentReservationHistory> EquipmentReservationHistories { get; set; }
        public List<EquipmentItem> EquipmentItems { get; set; }

    }
}
