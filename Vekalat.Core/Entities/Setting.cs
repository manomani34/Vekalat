using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Vekalat.Core.Common;
namespace Vekalat.Core.Entities
{
   public  class Setting:Entity
    {
        [MaxLength(50)]
        public string tel { get; set; }
        public string about { get; set; }
        public string ContactUs { get; set; }
        public string ghavanin { get; set; }

        [MaxLength(200)]
        public string Address { get; set; }

        [MaxLength(50)]
        public string Mobil { get; set; }
        public string Labout { get; set; }
    }
}
