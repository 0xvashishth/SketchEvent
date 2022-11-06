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
            List<Event> objEventList = _eventRepo.GetAllEvents().ToList();
            /*ViewBag.EventList = objEventList;*/
            return View(objEventList);
        }

/*        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUser(User objuser)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[12];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);
            objuser.Token = finalString;
            if (ModelState.IsValid)
            {
                if (_userRepo.GetExistingUser(objuser.Email, objuser.PhoneNo) != null)
                {
                    return RedirectToAction(nameof(Privacy));
                }
                _userRepo.Add(objuser);

                CookieOptions option = new CookieOptions();
                option.Expires = DateTime.Now.AddDays(10);

                Response.Cookies.Append("UserName", objuser.Name.ToString(), option);
                Response.Cookies.Append("UserEmail", objuser.Email.ToString(), option);
                Response.Cookies.Append("UserToken", objuser.Token, option);
                Response.Cookies.Append("UserId", objuser.UserId.ToString(), option);
                try
                {
                    ViewBag.erroronSignup = "Sucessfully Registered! Check Your Mail To Verify Your Account!";
                    *//*emailVerificationMailService objSendVerifyEmail = new emailVerificationMailService(objuser.Token.ToString(), objuser.Name, objuser.Email);
                    *//*return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }
            *//* var name = fc["user-name"];
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
             newUsr.Token = finalString;*//*
             return View();
        }*/

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