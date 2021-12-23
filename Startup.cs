using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseCalculator.DataLayer;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Hangfire;
using NidhiApplication.Models;

namespace ExpenseCalculator
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string AppMode = Configuration.GetSection("AppMode").Value.ToString();
            MyConnection.DefaultConnection = AppMode == "Test" ? Configuration["ConnectionStrings:Test_Connection"].ToString() : Configuration["ConnectionStrings:Live_Connection"].ToString();
            EmailMaster.HostServer = Configuration.GetSection(AppMode).GetSection("Email").GetSection("HostServer").Value.ToString();
            EmailMaster.Email = Configuration.GetSection(AppMode).GetSection("Email").GetSection("Email").Value.ToString();
            EmailMaster.Password = Configuration.GetSection(AppMode).GetSection("Email").GetSection("Password").Value.ToString();
            //EmailMaster.EnableSsl = Convert.ToBoolean(Configuration.GetSection("SSLEnable").Value);
            EmailMaster.Port = Convert.ToInt32(Configuration.GetSection(AppMode).GetSection("Email").GetSection("Port").Value);
            services.AddControllersWithViews();
            services.AddHttpContextAccessor();
            services.AddMediatR(typeof(Startup).Assembly);
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
            options =>
            {
                options.LoginPath = "/Home/Login";
                options.LogoutPath = "/Account/SignOut";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
            });
            services.AddHangfire(x => x.UseSqlServerStorage(AppMode == "Test" ? Configuration["ConnectionStrings:Test_Connection"].ToString() : Configuration["ConnectionStrings:Live_Connection"].ToString()));
            services.AddHangfireServer();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IBackgroundJobClient backgroundJobs)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new MyAuthorizationFilter() }
            });
            app.UseHangfireServer();

            RecurringJob.AddOrUpdate(() => ExpenseCalculator.Repos.ServerJobs.JobTesting1(), Cron.MinuteInterval(1));
            //RecurringJob.AddOrUpdate(() => ApplicationData.Master.ServerWork.InterestTransfer.RD_Account_Weekly_Interest_Transfer(), Cron.Weekly(DayOfWeek.Monday));
            //RecurringJob.AddOrUpdate(() => ApplicationData.Master.ServerWork.InterestTransfer.RD_Account_First_Fortnight_Interest_Transfer(), Cron.Monthly(16));
            //RecurringJob.AddOrUpdate(() => ApplicationData.Master.ServerWork.InterestTransfer.RD_Account_First_Fortnight_Interest_Transfer(), Cron.Monthly());
            //RecurringJob.AddOrUpdate(() => ApplicationData.Master.ServerWork.InterestTransfer.RD_Account_Monthly_Interest_Transfer(), Cron.Monthly());
            //RecurringJob.AddOrUpdate(() => ApplicationData.Master.ServerWork.InterestTransfer.RD_Account_First_Quarter_Interest_Transfer(), Cron.Yearly(07, 01));
            //RecurringJob.AddOrUpdate(() => ApplicationData.Master.ServerWork.InterestTransfer.RD_Account_Second_Quarter_Interest_Transfer(), Cron.Yearly(10, 01));
            //RecurringJob.AddOrUpdate(() => ApplicationData.Master.ServerWork.InterestTransfer.RD_Account_Third_Quarter_Interest_Transfer(), Cron.Yearly(01, 01));
            //RecurringJob.AddOrUpdate(() => ApplicationData.Master.ServerWork.InterestTransfer.RD_Account_Forth_Quarter_Interest_Transfer(), Cron.Yearly(04, 01));
            //RecurringJob.AddOrUpdate(() => ApplicationData.Master.ServerWork.InterestTransfer.RD_Account_First_Halfyear_Interest_Transfer(), Cron.Yearly(10, 01));
            //RecurringJob.AddOrUpdate(() => ApplicationData.Master.ServerWork.InterestTransfer.RD_Account_Second_Halfyear_Interest_Transfer(), Cron.Yearly(04, 01));
            //RecurringJob.AddOrUpdate(() => ApplicationData.Master.ServerWork.InterestTransfer.RD_Account_Yearly_Interest_Transfer(), Cron.Yearly(04, 01));


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Login}/{id?}");
            });
        }
    }
}
