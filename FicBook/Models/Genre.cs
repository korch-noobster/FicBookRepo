using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FicBook.Models
{
    public class Genre
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string GenreName { get; set; }
    }
}
