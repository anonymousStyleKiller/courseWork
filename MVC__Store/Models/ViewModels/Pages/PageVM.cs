using MVC__Store.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC__Store.Models.ViewModels.Pages
{
    //Page View Model
    public class PageVM
    {

        public PageVM() {}

        public PageVM(PagesDTO row) {
            Id = row.Id;
            Title = row.Title;
            Slug = row.Slug;
            Body = row.Body;
            Sorting = row.Sorting;
            HasSidebar = row.HasSidebar;
        }


        public int Id { get; set; }

        [Required]  
        //Что б тайтл был объязателен
        [StringLength(50,MinimumLength =3)]
        //Length тайтла

        public string Title { get; set; }
        public string Slug { get; set; }
        [Required]
        [StringLength(int.MaxValue, MinimumLength =3)]
        [AllowHtml]
        public string Body { get; set; }
        public int Sorting { get; set; }

        [Display(Name = "Sidebar")]
        public bool HasSidebar { get; set; }
    }
}