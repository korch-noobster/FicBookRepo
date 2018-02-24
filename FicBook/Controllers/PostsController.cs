using FicBook.Data;
using FicBook.Models;
using FicBook.Services;
using Imgur.API;
using Imgur.API.Authentication.Impl;
using Imgur.API.Endpoints.Impl;
using Imgur.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WkWrap.Core;

namespace FicBook.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IViewRenderService _viewRenderService;

        public PostsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IViewRenderService viewRenderService)
        {
            _context = context;
            _userManager = userManager;
            _viewRenderService = viewRenderService;
        }
        /*
        public IActionResult Pdf(PdfViewModel model)
        {
            return View(model);
        }
        public async Task<IActionResult> DownloadPdf(string id)
        {
            var model = new PdfViewModel()
            {
               Fanfik = _context.Posts
               // .Include(f => f.Chapters)
                .Include(f => f.Author)
                .FirstOrDefault(f => f.Id == id)
            };

            var htmlContent = await _viewRenderService.RenderToStringAsync("Posts/Pdf", model);
            var wkhtmltopdf = new FileInfo(@"C:\Program Files\wkhtmltopdf\bin\wkhtmltopdf.exe");
            var converter = new HtmlToPdfConverter(wkhtmltopdf);
            var pdfBytes = converter.ConvertToPdf(htmlContent);

            FileResult fileResult = new FileContentResult(pdfBytes, "application/pdf")
            {
              FileDownloadName = model.Fanfik.Title
            };
            return fileResult;
        }*/

        [Authorize]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Posts.SingleOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(Post post)
        {

            var parentPost = await _context.Posts.SingleOrDefaultAsync(m => m.Id == post.Id);
            if (post.TagString != null)
            {
                char[] delimeterChars = { ' ', ',' };
                string[] words = post.TagString.Split(delimeterChars);
                foreach (var word in words)
                {
                    if (await _context.Tags.FindAsync(word) == null)
                        post.Tags.Add(new Tag() { Name = word });
                }
            }
            parentPost.Content = post.Content;
            parentPost.Genre = post.Genre;
            parentPost.Abstract = post.Abstract;
            parentPost.TagString = post.TagString;
            parentPost.Title = post.Title;
            parentPost.LastModified = DateTime.UtcNow;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    
        [HttpPost]
        public async Task<IActionResult> AddComment(string postId,Post post)
        {
            var commentedPost = await _context.Posts.SingleOrDefaultAsync(m => m.Id == postId);
            //var commentAdded =_context.Comments.Add(new Comment() { Text = post.Comment, CreatedDate = DateTime.UtcNow, Author = await _userManager.GetUserAsync(User) });
            commentedPost.Comments.Add(new Comment() {
                Text = post.Comment,
                CreatedDate = DateTime.UtcNow,
                Author = await _userManager.GetUserAsync(User) ,
                Post =commentedPost,
                PostFk =post.ParentId
            });
            _context.SaveChanges();
            return Redirect("Details/"+postId);
        }

        [Authorize]
        public IActionResult AddChapter()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddChapter([Bind( "Title,Content,ParentId")] Post post)
        {
            if (ModelState.IsValid)
            {
                
                var parentPost = await _context.Posts.SingleOrDefaultAsync(m => m.Id == post.ParentId);
                post.Author = await _userManager.GetUserAsync(User);
                post.CreatedDate = DateTime.Now;
                parentPost.LastModified = DateTime.UtcNow;
                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(post);
        }
      
        public async Task<IActionResult> UploadImageAsync(IList<IFormFile> files)
         {

             try
             {
                 var user = await _userManager.GetUserAsync(User);
                 var client = new ImgurClient("556830a80ac5829", "9438948e5e7df4b5151a61b882626c499ef4925e");
                 var endpoint = new ImageEndpoint(client);
                 IImage image;
                 foreach (var file in files)
                 {
                     if (file.Length > 0)
                     {
                         user.AskVerified = true;
                         using (var fileStream = file.OpenReadStream())
                         using (var ms = new MemoryStream())
                         {
                             fileStream.CopyTo(ms);
                             var fileBytes = ms.ToArray();
                             string s = Convert.ToBase64String(fileBytes);
                             image = await endpoint.UploadImageBinaryAsync(fileBytes);
                         }
                         Debug.Write("Image uploaded. Image Url: " + image.Link);
                        ViewBag.img = image.Link;
                         return  Content(image.Link);
                     }
                 }
             }
             catch (ImgurException imgurEx)
             {
                 Debug.Write("An error occurred uploading an image to Imgur.");
                 Debug.Write(imgurEx.Message);
               
             }
            return null;
        }
       
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Posts.Include(a => a.Author);
            return View(await applicationDbContext.ToListAsync());
        }
       [HttpPost]
        public IActionResult SetLanguage(string page)
        {
            string culture = CultureInfo.CurrentCulture.Name == "ru" ? "en" : "ru";
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return Redirect(page);
        }

        public IActionResult ChangeTheme(string page)
        {
            
            if (Request.Cookies["theme"] == null)
            {
                Response.Cookies.Append("theme", "dark");
            }
            else
            {
                if (Request.Cookies["theme"] == "dark")
                {
                    Response.Cookies.Append("theme", "light");
                }
                else if (Request.Cookies["theme"] == "light")
                {
                    Response.Cookies.Append("theme", "dark");
                }
            }
            return Redirect(page);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.Include(a => a.Author).SingleOrDefaultAsync(m => m.Id == id);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Title, Abstract,Content,Genre,TagString")] Post post)
        {
            if (ModelState.IsValid)
            {
                char[] delimeterChars = {' ', ',' };
                string[] words = post.TagString.Split(delimeterChars);
                foreach(var word in words)
                {
                    post.Tags.Add(new Tag() { Name = word });
                }
                if (post.Picture == null)
                {
                    var img = await _context.Source.FindAsync("DefaultUser");
                    post.Picture = img.Picture;
                }
                post.LastModified = DateTime.UtcNow;
                post.ParentId = post.Id;
                post.Author = await _userManager.GetUserAsync(User);
                post.CreatedDate = DateTime.UtcNow;
                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(post);
        }

        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Posts.SingleOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            
            var article = await _context.Posts.SingleOrDefaultAsync(m => m.Id == id);
            if (article.ParentId == article.Id)
            {
                foreach (var remove in _context.Posts)
                    if (remove.ParentId == article.Id)
                        _context.Posts.Remove(article);

            }
            else
            {
                _context.Posts.Remove(article);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
