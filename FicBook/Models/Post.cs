using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FicBook.Models
{
    public class Post
    {
        [Required]
        public string ID { get; set; } = DateTime.UtcNow.Ticks.ToString();

        [Required]
        public string Title { get; set; }

        public string Slug { get; set; }

        [Required]
        public string Excerpt { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime PubDate { get; set; } = DateTime.UtcNow;

        public DateTime LastModified { get; set; } = DateTime.UtcNow;

        public bool IsPublished { get; set; } = true;

    }
}
