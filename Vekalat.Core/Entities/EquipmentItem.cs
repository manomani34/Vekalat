using Vekalat.Core.Entities;
using Vekalat.Application.Common;
using Vekalat.Application.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vekalat.Core.Entities
{
    public class EquipmentItem : AuditedEntity
    {
        public string SerialNumber { get; set; }
        public int EquipmentId { get; set; }

        [ForeignKey("EquipmentId")]
        public Equipment EquipmentFk { get; set; }

    }
}
