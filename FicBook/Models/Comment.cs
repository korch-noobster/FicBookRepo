using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FicBook.Models
{
    public class Comment
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public ApplicationUser Author { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public string PostId { get; set; }

        public int Likes { get; set; } = 0;

        public List<ApplicationUser> Liked { get; set; } = new List<ApplicationUser>();

        [Required]
        public string Text { get; set; }

    }
}
