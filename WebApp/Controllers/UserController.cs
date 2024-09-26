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
        private readonly AddressService _addressService;

        public UserController(UserService userService, AddressService addressService) : base()
        {
            _service = userService;
            _addressService = addressService;
        }

        // GET: User
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
            user.Address = _addressService.GetById((Guid)user.AddressId);
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
                TempData["Success"] = "Welcome!";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Error"] = "Error while registeting. Please try again or contact support@treolan.ru";
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
            user.Address = _addressService.GetById((Guid)user.AddressId);

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
                TempData["Success"] = "Saved changes";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Error"] = "Error while saving changes. Please try again or contact support@treolan.ru";
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
                TempData["Success"] = "User deleted";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["Error"] = "Error while deleting user. Please try again or contact support@treolan.ru";
                return RedirectToAction(nameof(Index));
            }
            
        }



    }
}
