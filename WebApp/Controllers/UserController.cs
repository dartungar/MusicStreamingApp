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
    public class UserController : Controller
    {
        private readonly UserService _service;

        public UserController(UserService userService) : base()
        {
            _service = userService;
        }

        // GET: User
        [Authorize]
        public ActionResult Index()
        {
            var users = _service.Get();
            return View(users);
        }

        // GET: User/Details/5
        [Authorize]
        public ActionResult Details(Guid id)
        {
            UserDto user = _service.GetById(id);
            if (user == null) return NotFound();
            return View(user);
        }

        // GET: User/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([FromForm]UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return View(userDto);
            }

            try
            {
                _service.Add(userDto);
                TempData["Success"] = "Пользователь зарегистрирован";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Error"] = "Ошибка при регистрации пользователя. Попробуйте еще раз или напишите на support@treolan.ru";
                return View();
            }
        }

        // GET: User/Edit/5
        [Authorize]
        public ActionResult Edit(Guid id)
        {
            UserDto user = _service.GetById(id);
            if (user == null)
                return NotFound();
            return View(user);
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([FromForm] UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                _service.Update(userDto);
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

        // GET: User/Delete/5
        [Authorize]
        public ActionResult Delete(Guid id)
        {
            try
            {
                _service.Delete(id);
                TempData["Success"] = "Пользователь удален";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["Error"] = "Ошибка при удалении пользователя. Попробуйте ещё раз или напишите на support@treolan.ru";
                return RedirectToAction(nameof(Index));
            }
            
        }

    }
}
