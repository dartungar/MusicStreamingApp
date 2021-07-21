﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Service;
using Service.DTO;
using Newtonsoft.Json.Serialization;


namespace WebApp.Controllers
{
    public class ArtistController : Controller
    {
        private readonly ArtistService _service;

        public ArtistController(ArtistService artistService) : base()
        {
            _service = artistService;
        }

        // GET: /artist
        public ActionResult Index()
        {
            var artists = _service.Get();
            return View(artists);
        }

        // API for Angular app
        public ActionResult List()
        {
            var artists = _service.Get();
            return Json(artists);
        }

        // GET: artist/Details/5
        public ActionResult Details(Guid id)
        {
            ArtistDto artist = _service.GetById(id);
            if (artist == null) return NotFound();
            return View(artist);
        }

        // GET: artist/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: artist/Create
        // API controller
        [HttpPost]
        // валидация токена мешает обработке запроса, но как именно - не смог понять
        // падает 400 не доходя до контроллера
        // [ValidateAntiForgeryToken] 
        //[Authorize]
        public ActionResult Create([FromBody]ArtistDto artistDto)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return StatusCode(401);
            }
            
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Incorrect data. Please fill in correct data and try again";
                return StatusCode(400);
            }


            try
            {
                _service.Add(artistDto);
                TempData["Success"] = "Created artist";
                return StatusCode(201);
            }
            catch
            {
                TempData["Error"] = "Error while creating artist. Please try again or contact support@treolan.ru";
                return StatusCode(500);
            }
        }

        // GET: artist/Edit/5
        [Authorize]
        public ActionResult Edit(Guid id)
        {
            ArtistDto artist = _service.GetById(id);
            if (artist == null)
                return NotFound();
            return View(artist);
        }

        // POST: artist/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize]
        public ActionResult Edit([FromForm] ArtistDto artistDto)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return StatusCode(401);
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                _service.Update(artistDto);
                TempData["Success"] = "Saved changes";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                // TODO: показывать сообщение об ошибке (передавать во вьюшку видимо? + в общем лэйауте место под это)
                TempData["Error"] = "Error while saving changes. Please try again or contact support@treolan.ru";
                return View();
            }
        }

        // GET: artist/Delete/5
        // [Authorize]
        public ActionResult Delete(Guid id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return StatusCode(401);
            }

            try
            {
                _service.Delete(id);
                TempData["Success"] = "Deleted artist";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["Error"] = "Error while deleting artist. Please try again or contact support@treolan.ru";
                return RedirectToAction(nameof(Index));
            }
            
        }

    }
}
