using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Vekalat.Core.Common;
using System.ComponentModel.DataAnnotations.Schema;
using Vekalat.Application.Common;

namespace Vekalat.Core.Entities
{
    public class Blog : AuditedEntity
    {
        public string Title { get; set; }
        public string Imagename { get; set; }
        public string Tag { get; set; }
        public string Description { get; set; }
        public int? LikeCount { get; set; }
        public int? ViewCount { get; set; }
        public string Abstract { get; set; }
        public bool IsVisible { get; set; }
        public int UserId { get; set; }
        public int BlogSubjectId { get; set; }

        [ForeignKey("BlogSubjectId")]
        public BlogSubject BlogSubjectFk { get; set; }

        [ForeignKey("UserId")]
        public User UserFk { get; set; }

    }
}
