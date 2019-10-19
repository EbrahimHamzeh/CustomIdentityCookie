using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using IdentityCookie.App.Models;
using System.Text;
using Microsoft.AspNetCore.Diagnostics;

namespace IdentityCookie.App.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Index(int? id)
        {
            var logBuilder = new StringBuilder();

            var statusCodeReExecuteFeature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            logBuilder.Append("Error ").Append(id).Append(" for").Append(Request.Method).Append(' ')
            .Append(statusCodeReExecuteFeature?.OriginalPath ?? Request.Path.Value).Append(Request.QueryString.Value).AppendLine("\n");

            var exceptionHandlerFeature = this.HttpContext.Features.Get<IExceptionHandlerFeature>();
            if (exceptionHandlerFeature?.Error != null)
            {
                var exception = exceptionHandlerFeature.Error;
                logBuilder.Append("<h1>Exception: ").Append(exception.Message).Append("</h1>").AppendLine(exception.StackTrace);
            }

            foreach (var header in Request.Headers)
            {
                var headerValues = string.Join(",", value: header.Value);
                logBuilder.Append(header.Key).Append(": ").AppendLine(headerValues);
            }

            _logger.LogError(logBuilder.ToString());

             if (id == null)
            {
                return View("Error");
            }

            switch (id.Value)
            {
                case 401:
                case 403:
                    return View("AccessDenied");
                case 404:
                    return View("NotFound");

                default:
                    return View("Error");
            }
        }
    }
}
