using System.ComponentModel.DataAnnotations;

namespace Vekalat.Application.Common.Dto.Enum
{
    public enum RequestTypeEnum
    {
        [Display(Name = "درخواست جدید")]
        NewRequest = 0,

        [Display(Name = "تایید شده")]
        Confirm = 1,

        [Display(Name = "رد شده")]
        Fail = 2,

        [Display(Name = "نامشخص")]
        FinalConfirm = 3,

        [Display(Name = "کلیه درخواستها")]
        All = 4,
    }
    public enum MemberTypeEnum
    {
        [Display(Name = "admin")]
        admin = 1,

        [Display(Name = "personnel")]
        personnel = 2,

        [Display(Name = "user")]
        user = 3,
    }


    public enum UserTypeEnum
    {
        [Display(Name = "Company")]
        Company = 1,

        [Display(Name = "Private")]
        Private = 2,
    }
}
