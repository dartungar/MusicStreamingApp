using Microsoft.AspNetCore.Http;
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
    public class ArtistController : Controller
    {
        private readonly ArtistService _service;

        public ArtistController(ArtistService artistService) : base()
        {
            _service = artistService;
        }

        // GET: ArtistController
        public ActionResult Index()
        {
            var artists = _service.Get();
            return View(artists);
        }

        // GET: ArtistController/Details/5
        public ActionResult Details(Guid id)
        {
            ArtistDto artist = _service.GetById(id);
            if (artist == null) return NotFound();
            return View(artist);
        }

        // GET: ArtistController/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: ArtistController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([FromForm]ArtistDto artistDto)
        {
            if (!ModelState.IsValid)
            {
                return View(artistDto);
            }

            try
            {
                _service.Add(artistDto);
                TempData["Success"] = "Исполнитель создан";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Error"] = "Ошибка при создании исполнителя. Попробуйте еще раз или напишите на support@treolan.ru";
                return View();
            }
        }

        // GET: ArtistController/Edit/5
        [Authorize]
        public ActionResult Edit(Guid id)
        {
            ArtistDto artist = _service.GetById(id);
            if (artist == null)
                return NotFound();
            return View(artist);
        }

        // POST: ArtistController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([FromForm] ArtistDto artistDto)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                _service.Update(artistDto);
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

        // GET: ArtistController/Delete/5
        [Authorize]
        public ActionResult Delete(Guid id)
        {
            try
            {
                _service.Delete(id);
                TempData["Success"] = "Исполнитель удален";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["Error"] = "Ошибка при удалении исполнителя. Попробуйте ещё раз или напишите на support@treolan.ru";
                return RedirectToAction(nameof(Index));
            }
            
        }

    }
}
