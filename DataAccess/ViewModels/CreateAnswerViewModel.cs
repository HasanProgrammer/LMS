using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Model;

namespace DataAccess.ViewModels
{
    public class CreateAnswerViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "فیلد پاسخ الزامی می باشد")]
        public string Content { get; set; }
    }
}