using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Vekalat.Core.Common;

namespace Vekalat.Core.Entities
{
    public class Category : Entity
    {
        [MaxLength(100)] 
        public string Title { get; set; }
        [MaxLength(100)]
        public string LTitle { get; set; }
        public int? ParentId { get; set; }

        public List<Category> Categories { get; set; }
    }
}
