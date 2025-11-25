using Microsoft.AspNetCore.Mvc;
using App.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Mvc.Controllers
{
    public class EventsController : Controller
    {
        private static List<EventModel> eventList = new()
        {
            new EventModel {
                Id = 1,
                Title = "Konser",
                Description = "Açýk hava konseri.",
                Date = DateTime.UtcNow.AddDays(7)
            },
            new EventModel {
                Id = 2,
                Title = "Konferans",
                Description = "Teknoloji konferansý.",
                Date = DateTime.UtcNow.AddMonths(1)
            },
        };

        public IActionResult List()
        {
            return View(eventList);
        }

        public IActionResult Details(int id)
        {
            var evt = eventList.FirstOrDefault(e => e.Id == id);
            if (evt == null) return NotFound();
            return View(evt);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(EventModel model)
        {
            if (ModelState.IsValid)
            {
                model.Id = eventList.Any() ? eventList.Max(e => e.Id) + 1 : 1;
                eventList.Add(model);
                return RedirectToAction("List");
            }
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var evt = eventList.FirstOrDefault(e => e.Id == id);
            if (evt == null) return NotFound();
            return View(evt);
        }

        [HttpPost]
        public IActionResult Edit(EventModel model)
        {
            var evt = eventList.FirstOrDefault(e => e.Id == model.Id);
            if (evt == null) return NotFound();
            if (ModelState.IsValid)
            {
                evt.Title = model.Title;
                evt.Description = model.Description;
                evt.Date = model.Date;
                return RedirectToAction("List");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var evt = eventList.FirstOrDefault(e => e.Id == id);
            if (evt != null)
                eventList.Remove(evt);
            return RedirectToAction("List");
        }
    }
}