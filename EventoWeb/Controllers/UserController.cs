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

        // GET: UserController
        public ActionResult Index()
        {
            return View();
        }

        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpRequest request, HttpResponse response)
        {/*
            var name = request["user-name"];*/
            User newUsr = new User();
            newUsr.Name = "Vasudev";
            newUsr.Email = "vasutemporarylc@gmail.com";
            newUsr.PhoneNo = "998856644";
            newUsr.Password = "pass";
            _userRepo.Add(newUsr);

            try
            {
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
            return View();
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
