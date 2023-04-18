using System.ComponentModel.DataAnnotations;

namespace Vekalat.Application.Enums
{
    public enum OrderTypeEnum
    {
        [Display(Name = "در حال جمع آوری")]
        Collecting = 1,

        [Display(Name = "آماده ارسال")]
        ReadyToSend = 2,

        [Display(Name = "ارسال شده")]
        Posted = 3,

        [Display(Name = "انصرافی")]
        refuse = 4,

        [Display(Name = "همه موارد")]
        All = 0,
    }
}
