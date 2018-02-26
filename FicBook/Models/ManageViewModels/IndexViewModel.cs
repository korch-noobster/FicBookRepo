using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FicBook.Models.ManageViewModels
{
    public class IndexViewModel
    {
        [Required]
        [StringLength(15, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

      
        [EmailAddress]
        public string Email { get; set; }

        public string StatusMessage { get; set; }
    }
}
