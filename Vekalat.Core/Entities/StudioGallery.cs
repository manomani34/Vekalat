using Vekalat.Application.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vekalat.Core.Entities
{
    public class StudioGallery : AuditedEntity
    {
        public string Title { get; set; }
        public int StudioId { get; set; }
        public string Imagename { get; set; }
        public bool DisplayFront { get; set; }

        [ForeignKey("StudioId")]
        public Studio StudioFk { get; set; }
    }
}
