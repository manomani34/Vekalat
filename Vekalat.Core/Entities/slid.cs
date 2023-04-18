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
    public class Slid : Entity
    {
        [MaxLength(100)]
        public string PictureURL { get; set; }

        [MaxLength(100)]
        public string Title { get; set; }

        public bool IsActive { get; set; } = false;

    }
}
