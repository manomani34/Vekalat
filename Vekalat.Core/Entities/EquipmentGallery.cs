using Vekalat.Application.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vekalat.Core.Entities
{
    public class EquipmentGallery : AuditedEntity
    {
        public string Title { get; set; }
        public int EquipmentId { get; set; }
        public string Imagename { get; set; }
        public bool DisplayFront { get; set; }


        [ForeignKey("EquipmentId")]
        public Equipment EquipmentFk { get; set; }
    }
}
