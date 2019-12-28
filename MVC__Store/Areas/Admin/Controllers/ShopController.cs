using MVC__Store.Models.Data;
using MVC__Store.Models.ViewModels.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MVC__Store.Areas.Admin.Controllers
{
    public class ShopController : Controller
    {
        // GET: Admin/Shop
        public ActionResult Categories()
        {

            // Обьявляем модель типа List
            List<CategoryVM> categoryVM_Lists;

            using (Db db = new Db())
            {
                //Инициализируем модель данных
                categoryVM_Lists = db.Categories.ToArray().OrderBy(x => x.Sorting).Select(x => new CategoryVM(x)).ToList();
            }
            // Возвращаем List в представление 
            return View(categoryVM_Lists);
        }


        // POST: Admin/Shop/AddNewCategory
        [HttpPost]
        public string AddNewCategory(string catName)
    {
        // Обьявляем строковую переменную ID
        string id;

        using (Db db = new Db())
        {
            // Проверяем имя категории на уникальность 
            if (db.Categories.Any(x => x.Name == catName))
            {
                return "titletaken";
            }

            // Инициализировать модель DTO
            CategoryDTO categoryDTO = new CategoryDTO();

            // Добавляем данные в модель
            categoryDTO.Name = catName;
            categoryDTO.Slug = catName.Replace(" ", "-").ToLower();
            categoryDTO.Sorting = 100;

            // Сохранить 
            db.Categories.Add(categoryDTO);
            db.SaveChanges();

            // Получить ID для возврата в представление
            id = categoryDTO.Id.ToString();
        }

        // Взвращаем ID
        return id;
    }
        // Создаем метод сортировки 
        // POST: Admin/Shop/ReorderCategories
        [HttpPost]
        public void ReorderCategories(int[] id)
        {
            using (Db db = new Db())
            {
                // Реадизуем начальный счётчик
                int count = 1;

                // Инициализируем модель данных 
                CategoryDTO categoryDTO;

                // Устанавливаем сортировку для каждой категории 
                foreach (var catId in id)
                {
                    categoryDTO = db.Categories.Find(catId);
                    categoryDTO.Sorting = count;

                    db.SaveChanges();

                    count++;
                }

            }
        }

        // Создаем метод удаления категории
        // GET: Admin/Shop/DeleteCategory
        public ActionResult DeleteCategory(int id)
        {
          
            using (Db db = new Db())
            {
                
                // Получаем модель категории 
                CategoryDTO categoryDTO = db.Categories.Find(id);
               
                // Удаляем категорию 
                db.Categories.Remove(categoryDTO);

                // Сохранине изминения в базе 
                db.SaveChanges();
            }

            // Добавляем сообщение о удачном удаление  
            TempData["SM"] = "You have deleted a page!";

            // Переадресовываем пользователя
            return RedirectToAction("Categories");
        }


        // Создаем метод удаления категории
        // POST: Admin/Shop/RenameCategory
        [HttpPost]
        public string RenameCategory(string newCatName, int id)
        {
            using (Db db = new Db())
            {
                // Проверяем имя на уникальность
                if (db.Categories.Any(x => x.Name == newCatName))
                    return "titletaken";

                // Получаем модель DTO
                CategoryDTO categoryDTO = db.Categories.Find(id);

                // Редактируем модель 
                categoryDTO.Name = newCatName;
                categoryDTO.Slug = newCatName.Replace(" ", "-").ToLower();

                // Сохранить 
                db.SaveChanges();
            }
            // Возвращаем слово 
            return "ok";

        }


    }
}
