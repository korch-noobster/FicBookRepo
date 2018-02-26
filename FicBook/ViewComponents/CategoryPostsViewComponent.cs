using FicBook.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FicBook.ViewComponents
{
    public class CategoryPostsViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public CategoryPostsViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string category)
        {
            var lastPost = await _context.Posts
                                            .Where(a => a.Genre == category)
                                            .Include(a => a.Author)
                                            .ToListAsync();
                                            
            return View(lastPost);
        }
    }
   
}
