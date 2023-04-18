using Vekalat.Core.Errors;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Claims;
using System.Text.Json;

namespace AryanShop.Application.Common.Helpers
{
    public static class ExtensionHelper
    {

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> query, string orderByExpression)
        {
            if (string.IsNullOrEmpty(orderByExpression))
                return query;

            string propertyName, orderByMethod;
            string[] strs = orderByExpression.Split(' ');
            propertyName = strs[0];

            if (strs.Length == 1)
                orderByMethod = "OrderBy";
            else
                orderByMethod = strs[1].Equals("des", StringComparison.OrdinalIgnoreCase) ? "OrderByDescending" : "OrderBy";

            ParameterExpression pe = Expression.Parameter(query.ElementType);
            MemberExpression me = Expression.Property(pe, propertyName);

            MethodCallExpression orderByCall = Expression.Call(typeof(Queryable), orderByMethod, new Type[] { query.ElementType, me.Type }, query.Expression
                , Expression.Quote(Expression.Lambda(me, pe)));

            return query.Provider.CreateQuery(orderByCall) as IQueryable<T>;
        }
        public static string GetFirstName(this ClaimsPrincipal claimsPrincipal)
        {
            var result = claimsPrincipal?.FindFirst("FirstName");
            return result == null ? default : result.Value.ToString();
        }
        public static string GetLastName(this ClaimsPrincipal claimsPrincipal)
        {
            var result = claimsPrincipal?.FindFirst("LastName");
            return result == null ? default : result.Value.ToString();
        }
        public static int GetMemberId(this ClaimsPrincipal claimsPrincipal)
        {
            var result = claimsPrincipal?.FindFirst("MemberId");
            return result == null ? default : Convert.ToInt32(result.Value);
        }
        public static int GetReturnedUserId(this ClaimsPrincipal claimsPrincipal)
        {
            var result = claimsPrincipal?.FindFirst("ReturnedUserId");
            return result == null ? -1 : Convert.ToInt32(result.Value);
        }
        public static bool GetIsAdmin(this ClaimsPrincipal claimsPrincipal)
        {
            var result = claimsPrincipal?.FindFirst("IsAdmin");
            return result != null && result.Value == "1";
        }

        public static int GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            var result = claimsPrincipal?.FindFirst("UserId");
            return result == null ? default : Convert.ToInt32(result.Value);
        }


        public static string ToBase64(this byte[] data)
        {
            if (data == null) return "";
            return Convert.ToBase64String(data);
        }



        public static string ToDateTypeString(this int? date)
        {
            if (date == null) { return ""; }
            DateTime dt;
            DateTime.TryParseExact(date.ToString(), "yyyyMMdd", null, DateTimeStyles.None, out dt);
            return $"{dt.Year}/{dt.Month:00}/{dt.Day:00}";
        }

        public static string ToShamsi(this DateTime date)
        {

            var pc = new PersianCalendar();
            return $"{pc.GetYear(date)}/{pc.GetMonth(date):00}/{pc.GetDayOfMonth(date):00}";
        }
        public static int ToYearShamsi(this DateTime date)
        {
            var pc = new PersianCalendar();
            return pc.GetYear(date);
        }
        public static string ToShamsiWithTime(this DateTime date)
        {
            var pc = new PersianCalendar();
            return $"{pc.GetYear(date)}/{pc.GetMonth(date):00}/{pc.GetDayOfMonth(date)}  {pc.GetHour(date)}:{pc.GetMinute(date)}";
        }
        public static string ToShamsi(this string date)
        {
            var dt = Convert.ToDateTime(date);
            var pc = new PersianCalendar();
            return $"{pc.GetYear(dt)}/{pc.GetMonth(dt):00}/{pc.GetDayOfMonth(dt)}";
        }

        public static int ToShamsiInt(this DateTime date)
        {

            var pc = new PersianCalendar();

            var str = $"{pc.GetYear(date)}{pc.GetMonth(date):00}{pc.GetDayOfMonth(date):00}";
            return int.Parse(str);
        }


        public static DateTime ToGregorian(this string persianStr)
        {
            var date = persianStr.ToLatingDigit();
            var splitedDate = date.Split('/');

            var pc = new PersianCalendar();
            var year = Convert.ToInt32(splitedDate[0]);
            var month = Convert.ToInt32(splitedDate[1]);
            var day = Convert.ToInt32(splitedDate[2]);

            var dt = new DateTime(year, month, day, pc);
            return dt;

        }
        public static string ToCalculateDiscountView(this decimal price, int discount)
        {
            var discountValue = price * discount / 100;
            return (price - discountValue).ToString();
        }
        public static decimal ToCalculateDiscount(this decimal price, int discount)
        {
            var discountValue = price * discount / 100;
            return price - discountValue;
        }
        public static TimeSpan ToGregorianTime(this string persianStr)
        {
            var convertedTime = persianStr.ToLatingDigit();
            return TimeSpan.Parse(convertedTime);

        }

        public static string ToFixTime(this TimeSpan persianStr)
        {
            return persianStr.ToString(@"hh\:mm");
        }
        public static string DummyData()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
        public static string ToEnumDisplayName(this Enum enu)
        {
            var attr = GetDisplayAttribute(enu);
            return attr != null ? attr.Name : enu.ToString();
        }
        private static DisplayAttribute GetDisplayAttribute(object value)
        {
            Type type = value.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException(string.Format("Type {0} is not an enum", type));
            }

            // Get the enum field.
            var field = type.GetField(value.ToString());
            return field?.GetCustomAttribute<DisplayAttribute>();
        }
        public static string ToFixedPath(this string path)
        {
            return path.Replace(" ", "-").Replace(".", " ").Replace("+", "-");
        }

        public static string ToFixedTextLength(this string text, int length)
        {
            if (!string.IsNullOrEmpty(text))
            {
                return (text.Length <= length) ? text : text.Substring(0, length) + "...";
            }
            else
            {
                return "";
            }
        }
        public static string ToLatingDigit(this string persianStr)
        {
            Dictionary<string, string> LettersDictionary = new Dictionary<string, string>
            {
                ["۰"] = "0",
                ["۱"] = "1",
                ["۲"] = "2",
                ["۳"] = "3",
                ["۴"] = "4",
                ["۵"] = "5",
                ["۶"] = "6",
                ["۷"] = "7",
                ["۸"] = "8",
                ["۹"] = "9"
            };
            return LettersDictionary.Aggregate(persianStr, (current, item) =>
                         current.Replace(item.Key, item.Value));
        }

        public static string ToPersianDigit(this string latinStr)
        {
            Dictionary<string, string> LettersDictionary = new Dictionary<string, string>
            {
                ["0"] = "۰",
                ["1"] = "۱",
                ["2"] = "۲",
                ["3"] = "۳",
                ["4"] = "۴",
                ["5"] = "۵",
                ["6"] = "۶",
                ["7"] = "۷",
                ["8"] = "۸",
                ["9"] = "۹"
            };
            return LettersDictionary.Aggregate(latinStr, (current, item) =>
                         current.Replace(item.Key, item.Value));
        }

        public static string ResultToJson(this object data, HttpContext context)
        {
            return JsonSerializer.Serialize(new
            {
                status = context.Response.StatusCode,
                result = new
                {
                    data
                }
            });
        }
        public static string ErrorToJson(this WebAppException wa)
        {

            return JsonSerializer.Serialize(new
            {
                status = wa.ErrorCode,
                target = wa.Error.Key,
                message = wa.Error.Value
            });
        }

        public static string ErrorToJson(this string err)
        {

            return JsonSerializer.Serialize(new
            {
                message = err
            });
        }

    }

}
