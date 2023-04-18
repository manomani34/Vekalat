using Vekalat.Core.Common;
using System;

namespace Vekalat.Application.Common
{
    public class AuditedEntity : Entity
    {
        public DateTime? CreationTime { get; set; }

        public int? CreatorId { get; set; }

        public DateTime? LastModifyTime { get; set; }

        public int? ModifierId { get; set; }
    }
}
