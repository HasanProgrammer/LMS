using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebFramework.Extensions
{
    public static class ModelExtension
    {
        public static JsonResult ErrorValidation(this ModelStateDictionary.ValueEnumerable values, int StatusCode, string Message)
        {
            List<string> errors = new List<string>();
            foreach (var error in values)
            {
                foreach (var item in error.Errors.Select(e => e.ErrorMessage))
                {
                    errors.Add(item);
                }
            }
            return new JsonResult(new
            {
                code = StatusCode,
                msg  = Message,
                body = new { errors }
            });
        }
    }
}