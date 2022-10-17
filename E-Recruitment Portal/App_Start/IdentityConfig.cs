using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using E_Recruitment_Portal.Models;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Net.Mime;

namespace E_Recruitment_Portal
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            await configSendGridasync(message);
            // return Task.FromResult(0);
        }

        // Use NuGet to install SendGrid (Basic C# client lib) 
        private async Task configSendGridasync(IdentityMessage message)
        {
            var _displayName = GeneralSetup.MailDisplayName;
            var _username = GeneralSetup.MailUsername;
            var _password = GeneralSetup.MailPassword;
            var _emailSign = GeneralSetup.EmailLogo;
            var _host = GeneralSetup.MailServer;
            var _port = GeneralSetup.MailPort;

            MailMessage msg = new MailMessage();
            LinkedResource res = new LinkedResource(System.Web.Hosting.HostingEnvironment.MapPath("~/Require/images/" + _emailSign));
            res.ContentId = Guid.NewGuid().ToString();
            msg.Body = message.Body.Replace("@ViewBag.Brand", @"<br><img src='cid:" + res.ContentId + @"'/><br>");
            AlternateView alternateView = AlternateView.CreateAlternateViewFromString(msg.Body, null, MediaTypeNames.Text.Html);
            alternateView.LinkedResources.Add(res);

            msg.From = new MailAddress(_username, _displayName);
            msg.To.Add(new MailAddress(message.Destination));
            msg.Subject = message.Subject;
            msg.IsBodyHtml = true;
            msg.AlternateViews.Add(alternateView);
            try
            {
                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.EnableSsl = true;
                    smtp.Host = _host;
                    smtp.Port = Convert.ToInt32(_port);
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential(_username, _password);

                    smtp.SendCompleted += (s, e) => { smtp.Dispose(); };
                    ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                    await smtp.SendMailAsync(msg);
                }
            }
            catch (Exception ex)
            {
                #region   ACTIVITY LOGS TXT
                var path = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Email_Logs/");
                String Todaysdate = DateTime.Now.ToString("dd-MMM-yyyy");
                if (!Directory.Exists(path + Todaysdate))
                {
                    Directory.CreateDirectory(path + Todaysdate);
                }
                using (StreamWriter writer = new StreamWriter(path + Todaysdate + "\\" + Todaysdate + ".txt", true))
                {
                    writer.WriteLine(DateTime.Now);
                }
                #endregion
                Console.Write(ex.StackTrace);
                await Task.FromResult(0);
            }
            await Task.FromResult(0);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context) 
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = 
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
