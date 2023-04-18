using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;

namespace Vekalat.Application.Common.Helpers
{
    public static class CookieHelper
    {
        public static void SetObjectAsJson(this IHttpContextAccessor context, string key, object value)
        {
            CookieOptions option = new()
            {
                Expires = DateTime.Now.AddDays(5)
            };
            //if (expireTime.HasValue)
            //    option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            var obj = JsonConvert.SerializeObject(value);
            context.HttpContext.Response.Cookies.Append(key, obj, option);
        }

        public static T GetObjectFromJson<T>(this IHttpContextAccessor context, string key)
        {
            var value = context.HttpContext.Request.Cookies[key];
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        public static bool ObjectExist(this IHttpContextAccessor context, string key)
        {
            var value = context.HttpContext.Request.Cookies[key];
            return value != null;
        }

        public static void RemoveObject(this IHttpContextAccessor context, string key)
        {
            context.HttpContext.Response.Cookies.Delete(key);
        }
    }
}
