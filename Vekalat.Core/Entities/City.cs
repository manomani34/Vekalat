using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Vekalat.Core.Common;

namespace Vekalat.Core.Entities
{
    public class City:Entity
    {
        [Required(ErrorMessage = "نباید بدون مقدار باشد!")]
        [MaxLength(100, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد!")]
        public string Title { get; set; }
        public int CountryId { get; set; }
        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }
    }
}
