using ExpenseCalculator.DataLayer;
using ExpenseCalculator.Repos.Command;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace ExpenseCalculator.Repos.Handler
{
    public class AccountHandler :   IRequestHandler<AccountLogin, object>,
                                    IRequestHandler<InsertExpence, object>
    {
        public IHttpContextAccessor _httpContextAccessor;
        public AccountHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<object> Handle(AccountLogin request, CancellationToken cancellationToken)
        {
            var data = (dynamic)null;
            try
            {
                if (!string.IsNullOrEmpty(request.UserName))
                {
                    if (!string.IsNullOrEmpty(request.Password))
                    {
                        var prm = new
                        {
                            UserName = request.UserName,
                            Password = request.Password
                        };
                        data = await dataLayer.QueryFirstOrDefaultAsyncWithDBResponse("AccountLogin", prm);
                        if (Convert.ToInt32(data.responseCode) == 200)
                        {
                            List<Claim> claims = new List<Claim>();
                            claims.Add(new Claim(ClaimTypes.Name, Convert.ToString(data.Id)));
                            claims.Add(new Claim(ClaimTypes.NameIdentifier, Convert.ToString(data.UserName)));
                            ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties { AllowRefresh = true, IsPersistent = true, ExpiresUtc = DateTime.Now.AddMinutes(60) });
                        }
                    }
                    else
                    {
                        data = ResponseResult.FailedResponse("Password Can't be Null");
                    }
                }
                else
                {
                    data = ResponseResult.FailedResponse("User Name Can't be Null");
                }
            }
            catch (Exception ex)
            {
                data = ResponseResult.ExceptionResponse("Internal Server Error..", ex.Message.ToString());
            }
            return data;
        }
        public async Task<object> Handle(InsertExpence request, CancellationToken cancellationToken)
        {
            var data = (dynamic)null;
            try
            {
                var Identity = (ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity;
                if (!string.IsNullOrEmpty(request.Title))
                {
                    if (!string.IsNullOrEmpty(request.Amount))
                    {
                        if (!string.IsNullOrEmpty(request.Date))
                        {
                            var prm = new
                            {
                                Title = request.Title,
                                Amount = request.Amount,
                                Date = request.Date,
                                CreatedBy = Identity.Name.ToString()
                            };
                            data = await dataLayer.QueryFirstOrDefaultAsyncWithDBResponse("InsertExpence", prm);
                        }
                        else
                        {
                            data = ResponseResult.FailedResponse("Date Can't be Null");
                        }
                    }
                    else
                    {
                        data = ResponseResult.FailedResponse("Amount Can't be Null");
                    }
                }
                else
                {
                    data = ResponseResult.FailedResponse("Title Can't be Null");
                }
            }
            catch (Exception ex)
            {
                data = ResponseResult.ExceptionResponse("Internal Server Error..", ex.Message.ToString());
            }
            return data;
        }
    }
}
