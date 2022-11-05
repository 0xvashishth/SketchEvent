using EventoWeb.MailServices;
using EventoWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EventoWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEventRepository _eventRepo;
        private readonly IUserRepository _userRepo;
        /*public string erroronsignup="Register First";*/

        public HomeController(ILogger<HomeController> logger, IEventRepository _eventRepo, IUserRepository _userRepo)
        {
            _logger = logger;
            this._eventRepo = _eventRepo;
            this._userRepo = _userRepo;
        }

        public IActionResult Index()
        {
            /*ViewBag.erroronSignup = this.erroronsignup;*/
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUser(IFormCollection fc)
        {
            var name = fc["user-name"];
            var email = fc["user-email"];
            var password = fc["user-password"];
            var phone = fc["user-phone"];
            User newUsr = new User();
            newUsr.Name = name;
            newUsr.Email = email;
            newUsr.PhoneNo = phone;
            newUsr.Password = password;
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[12];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);
            newUsr.Token = finalString;
            if(_userRepo.GetExistingUser(email, phone) != null)
            {
                return RedirectToAction(nameof(Privacy));
            }
            _userRepo.Add(newUsr);

            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(10);

            Response.Cookies.Append("UserName", newUsr.Name.ToString(), option);
            Response.Cookies.Append("UserEmail", newUsr.Email.ToString(), option);
            Response.Cookies.Append("UserToken", newUsr.Token, option);
            Response.Cookies.Append("UserId", newUsr.UserId.ToString(), option);
            try
            {
                ViewBag.erroronSignup = "Sucessfully Registered! Check Your Mail To Verify Your Account!";
                emailVerificationMailService objSendVerifyEmail = new emailVerificationMailService(newUsr.Token.ToString(), newUsr.Name, newUsr.Email);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginUser(IFormCollection fc)
        {
            var email = fc["user-email"];
            var password = fc["user-password"];
            var user = _userRepo.GetUser(email, password);
            if (user == null)
            {
                return RedirectToAction(nameof(Index));
            }
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(10);

            Response.Cookies.Append("UserName", user.Name.ToString(), option);
            Response.Cookies.Append("UserEmail", user.Email.ToString(), option);
            Response.Cookies.Append("UserToken", user.Token, option);
            Response.Cookies.Append("UserId", user.UserId.ToString(), option);
            try
            {
                ViewBag.erroronSignup = "Sucessfully LoggedIn!";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("UserEmail");
            Response.Cookies.Delete("UserName");
            Response.Cookies.Delete("UserToken");
            Response.Cookies.Delete("UserId");
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}