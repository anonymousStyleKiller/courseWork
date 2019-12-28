using MVC__Store.Models.Data;
using MVC__Store.Models.ViewModels.Pages;
using OpenXmlPowerTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;



namespace MVC__Store.Areas.Admin.Controllers
{
    public class PagesController : Controller
    {
        // GET: Admin/Pages
        public ActionResult Index()
        {
            //Обновляем список для представления (Pages)
            List<PageVM> pageList;


            //Инициализировать список (Db)
            using (Db db = new Db())
            {
                pageList = db.Pages.ToArray().OrderBy(x => x.Sorting).Select(x => new PageVM(x)).ToList();
            }


            // Возвращаем список представлния

            return View(pageList);
        }

        // Создаем метод добавления страниц
        // GET: Admin/Pages/AddPage
        [HttpGet]
        public ActionResult AddPage()
        {
            return View();
        }

        // Создаем метод добавления страниц
        // POST: Admin/Pages/AddPage
        [HttpPost]
        public ActionResult AddPage(PageVM model)
        {
            // Проверка модели на валидность  

            if (!ModelState.IsValid) {
                return View(model);
            }

            using (Db db = new Db()) {

                // Обьявляем переменную для краткого описания (slug)

                string slug;
                // Инициализруем класс PageDTO

                PagesDTO pagesDTO = new PagesDTO();

                // Присваеваем заголовок модели 
                pagesDTO.Title = model.Title.ToUpper();

                // Проверяем, есть ли краткое описание, если нет, присваеваем его 
                if (string.IsNullOrWhiteSpace(model.Slug))
                {
                    slug = model.Title.Replace(' ', '-').ToLower();
                }
                else
                {
                    slug = model.Slug.Replace(' ', '-').ToLower();
                }

                // Убеждаемся, что заголовок и краткое описание - уникальны 
                if (db.Pages.Any(x => x.Title == model.Title))
                {
                    ModelState.AddModelError("", "That title alredy exist");
                    return View(model);
                }

                else if (db.Pages.Any(x => x.Slug == model.Slug))
                {
                    ModelState.AddModelError("", "That slug alredy exist");
                    return View(model);
                }

                // Присваемваем оставшиеся значения модели 
                pagesDTO.Slug = slug;
                pagesDTO.Body = model.Body;
                pagesDTO.HasSidebar = model.HasSidebar;
                pagesDTO.Sorting = 100;

                // Сохраняем модель в базу
                db.Pages.Add(pagesDTO);
                db.SaveChanges();

            } //Using

            // Передаем через TempData
            TempData["SM"] = "You have added a new page!";

            // Пререадресоввываем пользователся на метод  INDEX
            return RedirectToAction("Index");
        }

        // Создаем метод редактирования страниц
        // GET: Admin/Pages/AddPage/id
        [HttpGet]
        public ActionResult EditPage(int id)
        {

            // Обьявляем модель PageVM
            PageVM model;

            using (Db db = new Db())
            {


                // Получаем страницу 
                PagesDTO pagesDTO = db.Pages.Find(id);
                // Проверяем, доступна ли страница 
                if (pagesDTO == null)
                {
                    return Content("The page does not exist.");
                }
                // Инициализируем модель данными
                model = new PageVM(pagesDTO);
            }
            // Возвращаем модель в представление 


            return View(model);
        }

        // Создаем метод редактирования страниц
        // POST: Admin/Pages/AddPage
        [HttpPost]
        public ActionResult EditPage(PageVM model)
        {

            // Проверить модель на Валидность 
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (Db db = new Db())
            {
                // Получаем id page 
                int id = model.Id;

                // Обьявим переменную для slug
                string slug = "home";

                // Получаем страницу по id 
                PagesDTO pagesDTO = db.Pages.Find(id);

                //  Присваеваем название из полученной модели  в DTO
                pagesDTO.Title = model.Title;

                //  Проверяем краткий заголовок и присваемваем его, если это необходимо 
                if (model.Slug != "home")
                {
                    if (string.IsNullOrWhiteSpace(model.Slug))
                    {
                        slug = model.Title.Replace(" ", "-").ToLower();
                    }
                    else
                    {
                        slug = model.Slug.Replace(" ", "-").ToLower();
                    }
                }
                // Проверяем slug and title на уникальность 
                if (db.Pages.Where(x => x.Id != id).Any(x => x.Title == model.Title))
                {
                    ModelState.AddModelError("", "That title already exist.");
                    return View(model);
                }

                else if (db.Pages.Where(x => x.Id != id).Any(x => x.Slug == slug)) {
                    ModelState.AddModelError("", "That title already exist.");
                    return View(model);
                }
                // Записываем остальные значение  в класс DTO
                pagesDTO.Slug = slug;
                pagesDTO.Body = model.Body;
                pagesDTO.HasSidebar = model.HasSidebar;
                // Сохраняем изменения в базу
                db.SaveChanges();
            }


            // Устанавливаем сообщение в TempData
            TempData["SM"] = "You have edited the page.";


            // Переадресация    пользователя 
            return View();
        }

        // Создаем метод деталей страниц
        // GET: Admin/Pages/PageDetails
        public ActionResult PageDetails(int id)
        {
            // Обьявляем модель PageVM
            PageVM pageVM;

            using (Db db = new Db())
            {
                // Получаем страницу 
                PagesDTO pagesDTO = db.Pages.Find(id);

                // Подтверждаем, что страница доступна 
                if (pagesDTO == null)
                {
                    return Content("The page does not exist.");
                }
                // Присваеваем модели информацию из базы 
                pageVM = new PageVM(pagesDTO);

            }
            // Возвращаем модель в представление 
            return View(pageVM);
        }

        // Создаем метод удаления страниц
        // GET: Admin/Pages/DeletePage
        public ActionResult DeletePage(int id)
        {
            using (Db db = new Db())
            {
                // Получаем страницу 
                PagesDTO pagesDTO = db.Pages.Find(id);
                // Удаляем страницу 
                db.Pages.Remove(pagesDTO);
                // Сохранине изминения в базе 
                db.SaveChanges();
            }

            // Добавляем сообщение о удачном удаление  
            TempData["SM"] = "You have deleted a page!";

            // Переадресовываем пользователя
            return RedirectToAction("Index");
        }

        // Создаем метод сортировки 
        // POST: Admin/Pages/ReorderPages
        [HttpPost]
        public void ReorderPages(int[] id)
        {
            using (Db db = new Db())
            {
                // Реадизуем начальный счётчик
                int count = 1;
                // Инициализируем модель данных 
                PagesDTO pagesDTO;
                // Устанавливаем сортировку для каждой страницы 
                foreach (var pageID in id)
                {
                    pagesDTO = db.Pages.Find(pageID);
                    pagesDTO.Sorting = count;

                    db.SaveChanges();

                    count++;
                }

            }
        }

        // Создаем сайтбар
        // GET: Admin/Pages/EditSidebar
        [HttpGet]
        public ActionResult EditSidebar()
        {
            // Обьявляем модель
            SidebarVM sidebarVM;

            using (Db db = new Db())
            {
                // Получаем данные из DTO
                SidebarDTO sidebarDTO = db.Sidebars.Find(1); // Говнокод! Жесткие значения в коде желательно не добавлять!!!

                // Заполняем модель данными 
                sidebarVM = new SidebarVM(sidebarDTO);
            }
            // Вернуть представление с моделью
            return View(sidebarVM);
        }

        // Создаем сайтбар
        // POST: Admin/Pages/EditSidebar
        [HttpPost]
        public ActionResult EditSidebar(SidebarVM sidebarVM)
        {
            using (Db db = new Db())
            {
                // Получаем данные из DTO
                SidebarDTO sidebarDTO = db.Sidebars.Find(1); // Говнокод! Жесткие значения в коде желательно не добавлять!!!

                // Присваиваем данные в тело
                sidebarDTO.Body = sidebarVM.Body;
                // Сохраняем
                db.SaveChanges();
            }
            // Сообщение в TempData
            TempData["SM"] = "You have edited the sidebar";
            // Переадрисация 
            return RedirectToAction("EditSidebar");
        }
    }
}