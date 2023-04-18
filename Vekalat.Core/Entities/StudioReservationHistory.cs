using Vekalat.Application.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vekalat.Core.Entities
{
    public class StudioReservationHistory : AuditedEntity
    {
        public int StudioId { get; set; }
        public DateTime ReservedDate { get; set; }
        public DateTime TakeDate { get; set; }
        public string Descrption { get; set; }

        [ForeignKey("StudioId")]
        public Studio StudioFk { get; set; }
    }
}
