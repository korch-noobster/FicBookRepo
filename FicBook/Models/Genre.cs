﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FicBook.Models
{
    public class Genre
    {
        [Key]
        public string GenreName { get; set; }
    }
}
