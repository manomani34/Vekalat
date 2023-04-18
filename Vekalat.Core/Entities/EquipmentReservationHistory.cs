using Vekalat.Application.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vekalat.Core.Entities
{
    public class EquipmentReservationHistory : AuditedEntity
    {
        public int EquipmentId { get; set; }
        public DateTime ReservedDate { get; set; }
        public DateTime TakeDate { get; set; }
        public string Descrption { get; set; }

        [ForeignKey("EquipmentId")]
        public Equipment EquipmentFk { get; set; }
    }
}
