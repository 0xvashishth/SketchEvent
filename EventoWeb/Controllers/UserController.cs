using EventoWeb.MailServices;
using EventoWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventoWeb.Controllers
{
    public class UserController : Controller
    {
        private readonly IEventRepository _eventRepo;
        private readonly IUserRepository _userRepo;
        public UserController(IEventRepository _eventRepo, IUserRepository _userRepo)
        {
            this._eventRepo = _eventRepo;
            this._userRepo = _userRepo;
        }

        public bool IsLoggedIn()
        {
            var UserName = HttpContext.Request.Cookies["UserName"];
            var UserEmail = HttpContext.Request.Cookies["UserEmail"];
            var UserToken = HttpContext.Request.Cookies["UserToken"];
            var UserId = HttpContext.Request.Cookies["UserId"];
            if (UserName == null || UserEmail == null || UserToken == null || UserId == null)
            {
                return false;
            }
            return true;
        }

        public bool IsOwner(string _UserId)
        {
            var UserName = HttpContext.Request.Cookies["UserName"];
            var UserEmail = HttpContext.Request.Cookies["UserEmail"];
            var UserToken = HttpContext.Request.Cookies["UserToken"];
            var UserId = HttpContext.Request.Cookies["UserId"];
            Console.WriteLine(_UserId);
            Console.WriteLine(UserEmail);
            Console.WriteLine(UserId);
            if (UserName == null || UserEmail == null || UserToken == null || UserId == null)
            {
                return false;
            }
            if (_UserId != UserId)
            {
                return false;
            }
            return true;
        }


        // GET: UserController
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            User objUser = _userRepo.GetUserById(id);
            if (objUser != null)
            {
                if (objUser.UserId.ToString() == HttpContext.Request.Cookies["UserId"]?.ToString() && IsLoggedIn())
                {
                    ViewBag.IsOwner = "true";
                    return View(objUser);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
/*        [ValidateAntiForgeryToken]*/
        public ActionResult Create(User newUsr)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[12];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);
            newUsr.Token = finalString;
            newUsr.IsVerified = false;
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Home");
            }
            if (_userRepo.GetExistingUser(newUsr.Email, newUsr.PhoneNo) != null)
            {
                return RedirectToAction(nameof(Create));
            }
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(10);

            try
            {
                emailVerificationMailService objSendVerifyEmail = new emailVerificationMailService(newUsr.Token.ToString(), newUsr.Name, newUsr.Email);
                newUsr = _userRepo.Add(newUsr);
                Response.Cookies.Append("UserName", newUsr.Name.ToString(), option);
                Response.Cookies.Append("UserEmail", newUsr.Email.ToString(), option);
                Response.Cookies.Append("UserToken", newUsr.Token, option);
                Response.Cookies.Append("UserId", newUsr.UserId.ToString(), option);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            if (!IsLoggedIn())
            {
                Console.WriteLine("Hello1");
                return RedirectToAction("Index", "Home");
            }
            User objUser = _userRepo.GetUserById(id);
            if (objUser != null)
            {
                if (objUser.UserId.ToString() == HttpContext.Request.Cookies["UserId"]?.ToString() && IsLoggedIn())
                {
                    /*ViewBag.IsOwner = "true";*/
                    return View(objUser);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User UserChanges)
        {
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(10);

            var UserId = HttpContext.Request.Cookies["UserId"];
            var UserName = HttpContext.Request.Cookies["UserName"];
            var UserEmail = HttpContext.Request.Cookies["UserEmail"];
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Privacy", "Home");
            }
            if (UserChanges.UserId.ToString() == HttpContext.Request.Cookies["UserId"]?.ToString() && IsLoggedIn())
                UserChanges = _userRepo.Update(UserChanges);
            else
            {
                return RedirectToAction("Privacy", "Home");
            }
            try
            {
                Response.Cookies.Append("UserName", UserChanges.Name.ToString(), option);
                Response.Cookies.Append("UserEmail", UserChanges.Email.ToString(), option);
                /*emailVerificationMailService objSendVerifyEmail = new emailVerificationMailService(UserChanges.Token.ToString(), UserChanges.Name, UserChanges.Email);
                */return RedirectToAction("Details", new { Id = UserChanges.UserId });
            }
            catch
            {
                return View();
            }
        }
    }
}
