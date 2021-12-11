using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirlineSystem.Controllers
{
    public class ErrorHandler : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult StatusError(int statusCode)
        {
            if (statusCode == 404)
            {
                ViewBag.Message = "СТРАНИЦА, КОТОРАЯ ВЫ ИЩЕТЕ, НЕ СУЩЕСТВУЕТ";
            }
            else
            {
                ViewBag.Message = "ЧТО-ТО БЫЛО НЕПРАВИЛЬНО ВО ВРЕМЯ ВАШЕГО ЗАПРОСА";
            }

            return View();
        }



        [AllowAnonymous]
        [Route("Error")]
        public IActionResult ExceptionError()
        {
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            ViewBag.ExceptionMessage = exceptionHandlerPathFeature.Error.Message;

            return View();
        }
    }
}
