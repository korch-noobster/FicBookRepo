using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
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
        
        [Required]
        public string Content { get; set; }

        public  ApplicationUser Author { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastModified { get; set; } = DateTime.UtcNow;

    }
}
