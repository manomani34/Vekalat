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
    public class Country : Entity
    {       
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        //public virtual ICollection<City> Citys { get; set; }
   
    }
}
