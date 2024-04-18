


using KutumbaBhoj.Application.Services;
using KutumbaBhoj.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;


namespace KutumbaBhoj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DishesController : ControllerBase
    {

        //injecting a Services
        private readonly IDishes _services;

        public DishesController(IDishes services)
        {
            _services = services;
        }

        //GET:api
        [HttpGet]
        public async Task<ActionResult<List<Dish>>> GetAllDishes()
        {
            var result = await _services.GetAllDishes();
            return Ok(result);
        }

        //Adding a dishes
        [HttpPost]
        public async Task<ActionResult<List<Dish>>> AddDishes(Dish I)
        {
            var result = await _services.AddDishes(I);
            return Ok(result);
        }

        //Uploading and downloading dishes image
        [HttpPost]
        [Route("UploadFile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadFile(IFormFile file, CancellationToken cancellationToken)
        {
            var result = await WriteFile(file);
            return Ok(result);
        }

        private async Task<string> WriteFile(IFormFile file)
        {
            string filename = "";
            try
            {
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                filename = DateTime.Now.Ticks.ToString() + extension;

                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files");

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                var exactpath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files", filename);
                using (var stream = new FileStream(exactpath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            catch (Exception ex)
            {

            }
            return filename;
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



        //Reading a single dish
        [HttpGet("{Id}")]
        public async Task<ActionResult<Dish>> GetSingleDish(int Id)
        {
            var result = await _services.GetSingleDish(Id);
            if (result is null)
                return NotFound("Item not found");

            return Ok(result);
        }

        //Updating a dish
        [HttpPut("{Id}")]
        public async Task<ActionResult<Dish>?> UpdateDish(int Id, Dish Request)
        {
            var result = await _services.UpdateDish(Id, Request);
            if (result is null)
                return NotFound("Dishes not found");

            return Ok(result);
        }

        //Deleting a dish
        [HttpDelete("{Id}")]
        public async Task<ActionResult<List<Dish>>?> DeleteDish(int Id)
        {
            var result = await _services.DeleteDish(Id);
            if (result is null)
                return null;

            return Ok(result);
        }

    }

}




