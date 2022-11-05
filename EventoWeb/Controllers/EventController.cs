using System;
using EventoWeb.Data;
using EventoWeb.MailServices;
using EventoWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventoWeb.Controllers
{
    public class EventController : Controller
    {
        private readonly IEventRepository _eventRepo;
        private readonly IUserRepository _userRepo;
        public EventController(IEventRepository _eventRepo, IUserRepository _userRepo)
        {
            this._eventRepo = _eventRepo;
            this._userRepo = _userRepo;
        }
        public bool IsLoggedIn() {
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
            if (UserName == null || UserEmail == null || UserToken == null || UserId == null)
            {
                return false;
            }
            if(_UserId != UserId)
            {
                return false;
            }
            return true;
        }

        // GET: EventController
        public ActionResult Index()
        {
            List<Event> objEventList = _eventRepo.GetAllEvents().ToList();
            return View(objEventList);
        }

        // GET: EventController/Details/5
        public ActionResult Details(int id)
        {
            Event objEvent = _eventRepo.GetEventById(id);
            if(objEvent == null)
            {
                return RedirectToAction("Index");
            }
            return View(objEvent);
        }

        // GET: EventController/Create
        public ActionResult Create()
        {
            if(IsLoggedIn() == true)
                return View();
            else
                return RedirectToAction("Index");
        }

        // POST: EventController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Event obj)
        {
            if (IsLoggedIn() == false)
                return RedirectToAction("Index");

            User newUsr = new User();
            newUsr.Name = "Vasudev";
            newUsr.Email = "vasutemporarylc@gmail.com";
            newUsr.PhoneNo = "998856644";
            newUsr.Password = "pass";
            _userRepo.Add(newUsr);
            obj.CreatedBy = newUsr;
            obj = _eventRepo.Add(obj);
            try
            {
                /*emailVerificationMailService objSendVerifyEmail = new emailVerificationMailService(newUsr.PhoneNo, newUsr.Name, newUsr.Email);*/
                return RedirectToAction("Details", new { Id = obj.EventId });
            }
            catch
            {
                return View();
            }
        }

        // GET: EventController/Edit/5
        public ActionResult Edit(int id)
        {
            var objEvent = _eventRepo.GetEventById(id);
            if (IsOwner(objEvent.CreatedBy.UserId.ToString()) == true && IsLoggedIn())
                return View(objEvent);
            else
                return RedirectToAction("Index");
        }

        // POST: EventController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Event EventChanges)
        {
            var owner = EventChanges.CreatedBy.UserId.ToString();
            if(IsOwner(owner) == true && IsLoggedIn())
                _eventRepo.Update(EventChanges);
            else
                return RedirectToAction("Index");
            try
            {
                return RedirectToAction("Details", new { Id = EventChanges.EventId });
            }
            catch
            {
                return View();
            }
        }

        // GET: EventController/Delete/5
        public ActionResult Delete(int id)
        {
            var objEvent = _eventRepo.GetEventById(id);
            if (IsOwner(objEvent.CreatedBy.UserId.ToString()) == true && IsLoggedIn())
                return View(objEvent);
            else
                return RedirectToAction("Index");
        }

        // POST: EventController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            var objEvent = _eventRepo.GetEventById(id);
            var owner = objEvent.CreatedBy.UserId.ToString();
            if (IsOwner(owner) == true && IsLoggedIn())
                _eventRepo.Delete(id);
            else
                return RedirectToAction("Index");
            try
            {
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
