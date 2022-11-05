using System;
using EventoWeb.Data;
using EventoWeb.MailServices;
using EventoWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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
            Console.WriteLine("Hello1");
            List<Event> objEventList = _eventRepo.GetAllEvents().ToList();
            return View(objEventList);
        }

        // GET: EventController/Details/5
        public ActionResult Details(int id)
        {
            Event objEvent = _eventRepo.GetEventById(id);
            Debug.Write("Hello");
            Debug.WriteLine(objEvent.CreatedBy);
            Debug.WriteLine(objEvent.CreatedById);
            if (objEvent == null)
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
            var Userid = Int32.Parse(HttpContext.Request.Cookies["UserId"]);
            var usr = _userRepo.GetUserById(Userid);
            obj.CreatedBy = usr;
            obj = _eventRepo.Add(obj);
            try
            {
                eventCreatedMailService objSendEventCreate = new eventCreatedMailService(usr.Name, obj.Name, "https://vashisht.co", obj.EndDate.ToString(), obj.EndDate.ToString(), obj.Venue, usr.Email);
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
            if (objEvent == null)
            {
                return RedirectToAction("Index");
            }
            Console.WriteLine(objEvent.Name);
            Console.WriteLine(objEvent.EndDate);
            Console.WriteLine(objEvent.CreatedById);
            if (IsOwner(objEvent.CreatedById.ToString()) == true && IsLoggedIn())
                return View(objEvent);
            else
                return RedirectToAction("Index");
        }

        // POST: EventController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Event EventChanges)
        {
            var owner = EventChanges.CreatedById.ToString();
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
            Event? objEvent = _eventRepo.GetEventById(id);
            if (objEvent == null)
            {
                return RedirectToAction("Index");
            }
            if (IsOwner(objEvent.CreatedById.ToString()) == true && IsLoggedIn())
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
            var owner = objEvent.CreatedById.ToString();
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
