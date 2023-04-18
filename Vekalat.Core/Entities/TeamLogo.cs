using Vekalat.Application.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vekalat.Core.Entities
{
    public class TeamLogo : AuditedEntity
    {
        public string Title { get; set; }
        public int TeamId { get; set; }
        public string Imagename { get; set; }

        [ForeignKey("TeamId")]
        public Team TeamFk { get; set; }
    }
}
