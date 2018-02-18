using FicBook.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FicBook.Models
{
    public class DbInitializer
    {
        public static async Task InitializeAsync(RoleManager<IdentityRole> roleManager,ApplicationDbContext _context)
        {

            if (await roleManager.FindByNameAsync("Admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            if (await roleManager.FindByNameAsync("Author") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Author"));
            }
#region GenresSeedig
            if(await _context.Genres.FindAsync("Action")==null)
            {
                await _context.AddAsync(new Genre() { GenreName = "Action"  });
            }
            if (await _context.Genres.FindAsync("Humour") == null)
            {
                await _context.AddAsync(new Genre() { GenreName = "Humour" });
            }
            if (await _context.Genres.FindAsync("Parody") == null)
            {
                await _context.AddAsync(new Genre() { GenreName = "Parody" });
            }
            if (await _context.Genres.FindAsync("Darkfic") == null)
            {
                await _context.AddAsync(new Genre() { GenreName = "Darkfic" });
            }
            if (await _context.Genres.FindAsync("Deathfic") == null)
            {
                await _context.AddAsync(new Genre() { GenreName = "Deathfic" });
            }
            if (await _context.Genres.FindAsync("POV") == null)
            {
                await _context.AddAsync(new Genre() { GenreName = "POV" });
            }
            if (await _context.Genres.FindAsync("Angst") == null)
            {
                await _context.AddAsync(new Genre() { GenreName = "Angst" });
            }
            if (await _context.Genres.FindAsync("Fluff") == null)
            {
                await _context.AddAsync(new Genre() { GenreName = "Fluff" });
            }
            if (await _context.Genres.FindAsync("Hurt/comfort") == null)
            {
                await _context.AddAsync(new Genre() { GenreName = "Fusion" });
            }
            if (await _context.Genres.FindAsync("ER") == null)
            {
                await _context.AddAsync(new Genre() { GenreName = "ER" });
            }
            if (await _context.Genres.FindAsync("PWP") == null)
            {
                await _context.AddAsync(new Genre() { GenreName = "Fix-fic" });
            }
#endregion 
            await _context.SaveChangesAsync();
        }
    }
}
