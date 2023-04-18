using Vekalat.Application.Common;
using Vekalat.Core.Errors;

namespace Vekalat.Core.Localization
{
    public static class Messages
    {

        public static string ExceptionHappened => $@"در انجام عملیات خطایی رخ داده است. لطفا بعدا امتحان کنید";
        public static string UnAuthorizedRequest => $@"کاربر گرامی شما دسترسی به این بخش از سایت را ندارید";


        public const string MaxResultCountError = "تعداد رکورد درخواستی مجاز نیست";
        public static string NationalCodeExist => $@"کد ملی وارد شده وجود دارد";
        public static string EmailExist => $@"ایمیل وارد شده وجود دارد";
        public static string UserExist => $@"نام کاربری وارد شده وجود دارد";
        public static string ValidationEmailCode => $@"کد یکتای وارد شده معتبر نمی باشد";
        public static string FileExtensionError => $@"فرمت فایل صحیح نمی یاشد";
        public static string FileSizeError => $@"حجم فایل وارد شده بیشتر از مقدار مجاز است";
        public static string UserNotFound => $@"نام کاربری یا کلمه عبور اشتباه است";
        public static string UserNotFoundEmail => $@"ناشری با ایمیل وارد شده در سایت ثبت نام نکرده است ";
        public static string EntityNotFound => $@"موجودیت یافت نشد ";
        public static string PersonalPhotoNotFount => $@"عکس پرسنلی وارد نشده است";
        public static string ResumeFileNotFount => $@"فایل رزومه وارد نشده است";
        public static string SubjectValidation => $@"موضوعات به درستی وارد نشده اند";
        public static string CurrentPasswordInvalid => $@"کلمه عبور فعلی صحیح نمی باشد";
        public static string ZarinPalFailRequest => $@"عدم اتصال به زرین پال";
        public static string VoucherExpireDate => $@"زمان استفاده از کد تخفیف بع پایان رسیده است";
        public static string VoucherUsageEnd => $@"تعداد استفده از کد تخفیف به اتمام رسیده است";
        public static string VoucherNotFound => $@"کد تخفیف یافت نشد";
        public static string VoucherSuccess => $@"کد تخفیف با موفقیت اعمال شد";
        public static string VoucherNotActive => $@"کد تخفیف فعال نمی باشد";
        public static string AddressNotFound => $@"لطفا از قسمت پروفایل آدرس ها  آدرسی  وارد نمایید";
        public static string AlreadyReserved => $@"Your Reservation range already taken by other. please choose another date";

        public static ReturnedDto InvalidState => new() { Message = "مقادیر به درستی وارد نشده اند", Status = 422 };
        public static ReturnedDto SuccessState => new() { Message = "عملیات با موفقیت انجام شد", Status = 200 };
        public static ReturnedDto FailState => new() { Message = ExceptionHappened, Status = 500 };
        public static ReturnedDto FailExceptionState(WebAppException e) => new() { Message = e.Error.Value, Status = (int)e.ErrorCode };

    }

    public static class Messages<T> where T : class
    {
        public static ReturnedDto<T> InvalidState => new() { Message = "مقادیر به درستی وارد نشده اند", Status = 422 };
        public static ReturnedDto<T> FailState(T data) => new() { Message = $@"در انجام عملیات خطایی رخ داده است. لطفا بعدا امتحان کنید", Status = 500, Result = data };
        public static ReturnedDto<T> SuccessState(T data) => new() { Message = "عملیات با موفقیت انجام شد", Status = 200, Result = data };
        public static ReturnedDto<T> FailExceptionState(WebAppException e) => new() { Message = e.Error.Value, Status = (int)e.ErrorCode };

    }

}
