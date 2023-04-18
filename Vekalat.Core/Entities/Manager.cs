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
    public class Manager : Entity
    {
        [Required]
        [MaxLength(100)]
        public string ManagerName { get; set; }

        [MaxLength(100)]
        public string Title { get; set; }
        
        [MaxLength(100)]
        public string PTitle { get; set; }

        [MaxLength(100)]
        public string Pic { get; set; }

        [MaxLength(11)] 
        public string Mobil { get; set; }
        public int? PublisherId { get; set; }
        public int? radif { get; set; }
        public int Dore { get; set; }
    }
}

