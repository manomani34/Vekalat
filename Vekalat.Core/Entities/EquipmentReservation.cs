using Vekalat.Application.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vekalat.Core.Entities
{
    public class EquipmentReservation : AuditedEntity
    {
        public string Title { get; set; }
        public int EquipmentId { get; set; }
        public int UserId { get; set; }
        public DateTime ReservedDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public string Description { get; set; }


        [ForeignKey("EquipmentId")]
        public Equipment EquipmentFk { get; set; }

        [ForeignKey("UserId")]
        public User UserFk { get; set; }
    }
}
