using System;
using System.ComponentModel.DataAnnotations;

namespace Foras_Khadra.Models
{
    public class Article
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public string ImagePath { get; set; }

        public DateTime PublishDate { get; set; }

        public string Author { get; set; }
    }
}
