﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Service;
using Service.DTO;

namespace WebApp.Controllers
{
    public class TrackController : Controller
    {
        private readonly TrackService _service;

        public TrackController(TrackService service) : base()
        {
            _service = service;
        }

        // GET: TrackController
        public ActionResult Index()
        {
            var Tracks = _service.Get();
            return View(Tracks);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(string query)
        {
            var Tracks = _service.Get(t => 
                t.Name == query  || 
                t.Album.Name == query ||
                t.TrackArtists.Where(ta => ta.Artist.Name == query).Any()
                );
            return View(Tracks);
        }

        // GET: TrackController/Details/5
        public ActionResult Details(Guid id)
        {
            TrackDto Track = _service.GetById(id);
            if (Track == null) return NotFound();
            return View(Track);
        }

        // GET: TrackController/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: TrackController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([FromForm] TrackDto TrackDto)
        {
            if (!ModelState.IsValid)
            {
                return View(TrackDto);
            }

            try
            {
                _service.Add(TrackDto);
                TempData["Success"] = "Трек создан";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Error"] = "Ошибка при создании трека. Попробуйте еще раз или напишите на support@treolan.ru";
                return View();
            }
        }

        // GET: TrackController/Edit/5
        [Authorize]
        public ActionResult Edit(Guid id)
        {
            TrackDto Track = _service.GetById(id);
            if (Track == null)
                return NotFound();
            return View(Track);
        }

        // POST: TrackController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([FromForm] TrackDto TrackDto)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                _service.Update(TrackDto);
                TempData["Success"] = "Изменения сохранены";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                // TODO: показывать сообщение об ошибке (передавать во вьюшку видимо? + в общем лэйауте место под это)
                TempData["Error"] = "Ошибка при сохранении изменений. Попробуйте ещё раз или напишите на support@treolan.ru";
                return View();
            }
        }

        // GET: TrackController/Delete/5
        [Authorize]
        public ActionResult Delete(Guid id)
        {
            try
            {
                _service.Delete(id);
                TempData["Success"] = "Трек удален";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["Error"] = "Ошибка при удалении трека. Попробуйте ещё раз или напишите на support@treolan.ru";
                return RedirectToAction(nameof(Index));
            }

        }
    }
}
