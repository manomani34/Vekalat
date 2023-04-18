using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Vekalat.Core.Common;

namespace Vekalat.Core.Entities
{
   public  class ContactMessage:Entity 
    {
        [Required]
        [MaxLength(50)]
        [Display(Name = "نام")]
        public string Name { get; set; }

        [MaxLength(100)]
        [Display(Name = "پست الکترونیک")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "متن پیام")]
        public string Message { get; set; }

        [Required]
        [Display(Name = "جواب داده شده")]
        public bool IsRespone { get; set; }

        [Required]
        [MaxLength(10)]
        [Display(Name = "تاریخ ارسال")]
        public string Fdate { get; set; }

       
        [Display(Name = "پاسخ")]
        public string respone { get; set; }

        [MaxLength(100)]
        [Display(Name = "پاسخ دهنده")]
        public string ResponeUser { get; set; }

        [MaxLength(11)]
        [Display(Name = "موبایل	")]
        public string Mobil { get; set; }

        [MaxLength(10)]
        [Display(Name = "تاریخ پاسخ	")]
        public string Rdate { get; set; }
    }
}
