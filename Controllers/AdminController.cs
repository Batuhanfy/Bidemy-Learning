﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UdemyEgitimPlatformu.Data;
using Microsoft.EntityFrameworkCore;
using BidemyLearning.Controllers;
using UdemyEgitimPlatformu.ViewModel;
using UdemyEgitimPlatformu.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;

namespace UdemyEgitimPlatformu.Controllers
{
    public class AdminController : Controller
    {

        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;




        }

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var KategoriListGenel = _context.Kategoriler.ToList();
            var Settings_Ayarlar = _context.Settings.ToList();

            var BirlestirilmisViewModel = new CompositeViewModel
            {
                CategoryViewModel = new CategoryViewModel
                {
                    KategoriListGenel = KategoriListGenel,
                    Ayarlar = Settings_Ayarlar,
                }
            };

            return View(BirlestirilmisViewModel);



        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Menuler()
        {
            var KategoriListGenel = _context.Kategoriler.ToList();
            var Settings_Ayarlar = _context.Settings.ToList();





            var menuler = await _context.Menuler.ToListAsync();
            var kategoriler = await _context.Kategoriler.ToListAsync();

            var viewModel = new CompositeViewModel
            {
                CategoryViewModel = new CategoryViewModel
                {
                    KategoriListGenel = KategoriListGenel,
                    Ayarlar = Settings_Ayarlar,
                },
                Menuler = menuler,
                KategorilerList = kategoriler
            };

            return View(viewModel);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Update(CompositeViewModel model)
        {
            
                foreach (var menu in model.Menuler)
                {
                    var existingMenu = _context.Menuler.FirstOrDefault(m => m.Id == menu.Id);
                    if (existingMenu != null)
                    {
                        existingMenu.CategoryId = menu.CategoryId;
                        existingMenu.Name = menu.Name;
                        existingMenu.Description = menu.Description;
                        existingMenu.IsEnable = menu.IsEnable;
                    }
                }

                _context.SaveChanges();
                TempData["success"] = "true";
                TempData["message"] = "Menüler başarıyla güncellendi.";
            
           

            return RedirectToAction("Menuler");
        }


        [Authorize(Roles = "Admin")]
        public ActionResult Kullanicilar()
        {
            var KategoriListGenel = _context.Kategoriler.ToList();
            var Settings_Ayarlar = _context.Settings.ToList();

            var Kullanicilar = _context.Users.ToList();

            var BirlestirilmisViewModel = new CompositeViewModel
            {
                ApplicationUsers = Kullanicilar,
                CategoryViewModel = new CategoryViewModel
                {
                    KategoriListGenel = KategoriListGenel,
                    Ayarlar = Settings_Ayarlar,
                }
            };

            return View(BirlestirilmisViewModel);
        }



        [Authorize(Roles = "Admin")]
        public ActionResult GetSettings()
        {
            var KategoriListGenel = _context.Kategoriler.ToList();
            var Settings_Ayarlar = _context.Settings.ToList();

           

            var BirlestirilmisViewModel = new CompositeViewModel
            {
                CategoryViewModel = new CategoryViewModel
                {
                    KategoriListGenel = KategoriListGenel,
                    Ayarlar = Settings_Ayarlar,
                }
            };

            return View(BirlestirilmisViewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult SmtpSettings(int id, string host, string port, string username, string password, string konu, string mesaj, bool EnableSsl)
        {
            var check = false;

            // Güncelleme işlemini gerçekleştirin
            var setting = _context.SmtpSettings.FirstOrDefault(s => s.Id == id);
            if (setting != null)
            {
                setting.Host = host;
                setting.Port = int.Parse(port);
                setting.Username = username;
                setting.Password = password;
                setting.Konu = konu;
                setting.Mesaj = mesaj;
                setting.EnableSsl = EnableSsl;
                _context.SaveChanges();

                check = true;
            }

            if (check)
            {
                TempData["success"] = "true";
                TempData["message"] = "Başarıyla güncellendi.";
            }
            else
            {
                TempData["success"] = "false";
                TempData["message"] = "Bir hata oluştu.";
            }
            return RedirectToAction("GetSettings", "Admin");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Loglar()
        {
            var KategoriListGenel = _context.Kategoriler.ToList();
            var Settings_Ayarlar = _context.Settings.ToList();

            var loglar = _context.Logs.ToList();

            var BirlestirilmisViewModel = new CompositeViewModel
            {
                Loglar = loglar,
                CategoryViewModel = new CategoryViewModel
                {
                    KategoriListGenel = KategoriListGenel,
                    Ayarlar = Settings_Ayarlar,
                }
            };

            return View(BirlestirilmisViewModel);
        }


            [Authorize(Roles = "Admin")]
        public ActionResult SmtpSettings()
        {

            var KategoriListGenel = _context.Kategoriler.ToList();
            var Settings_Ayarlar = _context.Settings.ToList();
            var SmtpSettings = _context.SmtpSettings.ToList();
            var BirlestirilmisViewModel = new CompositeViewModel
            {
                SmtpSettings =  SmtpSettings,
                CategoryViewModel = new CategoryViewModel
                {
                    KategoriListGenel = KategoriListGenel,
                    Ayarlar = Settings_Ayarlar,
                }
            };

            return View(BirlestirilmisViewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Guncelle(int id,string value)
        {

            var check = false;
            // Güncelleme işlemini gerçekleştirin
            var setting = _context.Settings.FirstOrDefault(s => s.Id == id);
            if (setting != null)
            {
                setting.Value = value;
                _context.SaveChanges();

                check = true;
            }

            if (check)
            {
                TempData["success"] = "true";
                TempData["message"] = "Başarıyla güncellendi.";
            }
            else
            {
                TempData["success"] = "false";
                TempData["message"] = "Bir hata oluştu.";
            }
            return RedirectToAction("GetSettings", "Admin");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AdminController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AdminController/Edit/5
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

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
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
