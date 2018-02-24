using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FicBook.Models
{
    public class Tag
    {

        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; }
        
    }
}
