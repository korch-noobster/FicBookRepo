using System.IO;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FicBook.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Imgur.API.Authentication.Impl;
using Imgur.API.Endpoints.Impl;
using Imgur.API.Models;
using System;
using System.Diagnostics;
using Imgur.API;
using FicBook.Models.ManageViewModels;

namespace FicBook.Controllers
{
    public class ImageController : Controller
    {
       
        private readonly UserManager<ApplicationUser> _userManager;

        public ImageController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [Authorize]
        public async Task UploadImageAsync(IList<IFormFile> files)
        {
            
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
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
                        user.ProfilePicture = image.Link;
                        await _userManager.UpdateAsync(user);
                    }
                }
            }
            catch (ImgurException imgurEx)
            {
                Debug.Write("An error occurred uploading an image to Imgur.");
                Debug.Write(imgurEx.Message);
            }
        }
    }
}