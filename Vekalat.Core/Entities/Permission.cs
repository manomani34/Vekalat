using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Vekalat.Core.Common;

namespace Vekalat.Core.Entities
{
    public class Permission : Entity
    {

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

    }
}
