using System;
using System.ComponentModel.DataAnnotations;

namespace Foras_Khadra.Models
{
    public class Article
    {
        public int Id { get; set; }

        [Required]
        public string TitleAr { get; set; }

        [Required]
        public string TitleEn { get; set; }

        [Required]
        public string TitleFr { get; set; }

        [Required]
        public string ContentAr { get; set; }

        [Required]
        public string ContentEn { get; set; }

        [Required]
        public string ContentFr { get; set; }

        public string ImagePath { get; set; }

        public DateTime PublishDate { get; set; }

        public string Author { get; set; }
    }
}
