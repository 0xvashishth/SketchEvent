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
        public string erroronsignup="Register First";

        public HomeController(ILogger<HomeController> logger, IEventRepository _eventRepo, IUserRepository _userRepo)
        {
            _logger = logger;
            this._eventRepo = _eventRepo;
            this._userRepo = _userRepo;
        }

        public IActionResult Index()
        {
            ViewBag.erroronSignup = this.erroronsignup;
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

            _userRepo.Add(newUsr);

            try
            {
                this.erroronsignup = "Sucessfully Registered! Check Your Mail To Verify Your Account!";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}