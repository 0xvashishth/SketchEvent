using System;
using EventoWeb.Data;
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
            Console.WriteLine(id);
            return View(objEvent);
        }

        // GET: EventController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EventController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Event obj)
        {
            User newUsr = new User();
            newUsr.Name = "Vasudev";
            newUsr.Email = "Vashisth@gmail.com";
            newUsr.PhoneNo = "998856644";
            newUsr.Password = "pass";
            _userRepo.Add(newUsr);
            obj.CreatedBy = newUsr;
            obj = _eventRepo.Add(obj);
            try
            {
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
            return View(objEvent);
        }

        // POST: EventController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Event EventChanges)
        {
            _eventRepo.Update(EventChanges);
            try
            {
                return RedirectToAction("Details", new {Id= EventChanges.EventId});
            }
            catch
            {
                return View();
            }
        }

        // GET: EventController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EventController/Delete/5
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
