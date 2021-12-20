using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ExpenseCalculator.Models;
using MediatR;
using ExpenseCalculator.Repos.Command;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using ExpenseCalculator.DataLayer;

namespace ExpenseCalculator.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMediator _m;

        public HomeController(IMediator mediator)
        {
            _m = mediator;
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AccountLogin([FromBody] AccountLogin accountLogin)
        {
            var data = await _m.Send(accountLogin);
            return Ok(data);
        }
        public IActionResult Dashboard()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> SignOut()
        {
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Login");
            }
            catch
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public async Task<IActionResult> InsertExpence([FromBody] InsertExpence insertExpence)
        {
            var data = await _m.Send(insertExpence);
            return Ok(data);
        }
        [HttpGet]
        public IActionResult SendMail()
        {
            var data = (dynamic)null;
            data = Utility.SendEmail("yatul360@gmail.com", "Test Subject1", "Test Body1");
            return Ok(data);
        }

    }
}
