using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FicBook.Models
{
    public class Comment
    {
        public int CommentId { get; set; }

        public ApplicationUser Author { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public Post CommentedPost { get; set; }
        [Required]
        public string Text { get; set; }

    }
}
