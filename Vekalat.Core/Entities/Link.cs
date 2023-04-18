using Vekalat.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vekalat.Core.Entities
{
    public class Link : Entity
    {

        [MaxLength(300)]
        public string Title { get; set; }
        public int Order { get; set; }
        [MaxLength(300)]
        public string Url { get; set; }
        public bool IsActive { get; set; } = false;

    }
}
