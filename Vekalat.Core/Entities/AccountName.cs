using Vekalat.Core.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Vekalat.Core.Entities
{
    public  class AccountName:Entity
    {
        [MaxLength(200)] 
        public string Title { get; set; }
    }
}
