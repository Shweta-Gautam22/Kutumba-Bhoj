using KutumbaBhoj.Application.Services;
using KutumbaBhoj.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Threading;

namespace KutumbaBhoj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurants _services;

        public RestaurantController(IRestaurants services)
        {
            _services = services;
        }

        //GET:api
        [HttpGet]
        public async Task<ActionResult<List<Restaurant>>> GetAllRestaurants()
        {
            var result = await _services.GetAllRestaurants();
            return Ok(result);
        }

        //Adding a restaurants
        [HttpPost]
        public async Task<ActionResult<List<Restaurant>>> AddRestaurants(Restaurant I)
        {
           
            var result = await _services.AddRestaurants(I);
            return Ok(result);
        }

        //Uploading and Downloading restaurant logo
        [HttpPost]
        [Route("UploadFile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadFile(IFormFile logoFile)
        {
            try
            {
                if (logoFile == null || logoFile.Length == 0)
                {
                    return BadRequest("File is empty");
                }

                string filename = DateTime.Now.Ticks.ToString() + Path.GetExtension(logoFile.FileName);
                string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files");
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                string filePath = Path.Combine(directoryPath, filename);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await logoFile.CopyToAsync(stream);
                }

                return Ok(filename);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error uploading file");
            }
        }


        [HttpGet]
        [Route("DownloadFile")]
        public async Task<IActionResult> DownloadFile(string filename)
        {
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files", filename);

            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filename, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            var bytes = await System.IO.File.ReadAllBytesAsync(filepath);
            return File(bytes, contentType, Path.GetFileName(filepath));
        }

        //Reading a single restaurant
        [HttpGet("{Id}")]
        public async Task<ActionResult<Restaurant>> GetSingleRestaurant(int Id)
        {
            var result = await _services.GetSingleRestaurant(Id);
            if (result is null)
                return NotFound("Item not found");

            return Ok(result);
        }

        //Updating a food
        [HttpPut("{Id}")]
        public async Task<ActionResult<Restaurant>?> UpdateRestaurant(int Id, Restaurant Request)
        {
            var result = await _services.UpdateRestaurant(Id, Request);
            if (result is null)
                return NotFound("Restaurants not found");

            return Ok(result);
        }

        //Deleting a restaurant
        [HttpDelete("{Id}")]
        public async Task<ActionResult<List<Restaurant>>?> DeleteRestaurant(int Id)
        {
            var result = await _services.DeleteRestaurant(Id);
            if (result is null)
                return null;

            return Ok(result);
        }
    }
}
