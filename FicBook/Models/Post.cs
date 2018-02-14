using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FicBook.Models
{
    public class Post
    {
        
        public int ID { get; set; } 

        [Required]
        public string Title { get; set; }

        [Required]
        public string Abstract { get; set; }
        // public string Slug { get; set; }
        [Required]
        public string Content { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastModified { get; set; } = DateTime.UtcNow;

        public bool IsPublished { get; set; } = true;

    }
}
