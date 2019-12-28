using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MVC__Store.Models.Data
{
    //Подключаемся к нужной таблице, using System.ComponentModel.DataAnnotations.Schema;

    [Table("tbPages")]
    public class PagesDTO
    {
        //Указывает первичный ключ, using System.ComponentModel.DataAnnotations;

        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Body { get; set; }
        public int Sorting { get; set; }

        public bool HasSidebar { get; set; }
    }
}