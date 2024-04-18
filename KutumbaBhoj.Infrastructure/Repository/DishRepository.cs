using KutumbaBhoj.Application.Interfaces;
using KutumbaBhoj.Infrastructure.DbContext;
using KutumbaBhoj.Domain.Models;
using Microsoft.EntityFrameworkCore;


namespace KutumbaBhoj.Infrastructure.Repository
{
    public class DishRepository:IDishRepository
    {
        private readonly DataContext _dbContext;

        public DishRepository(DataContext DbContext)
        {
            _dbContext = DbContext;
        }

        public async Task<List<Dish>> GetAllDishes()
        {
            return await _dbContext.Dishes.ToListAsync();
        }

        public async Task<List<Dish>> AddDishes(Dish i)
        {
            if (i.ImageFile != null && i.ImageFile.Length > 0)
            {
                var extension = Path.GetExtension(i.ImageFile.FileName);
                var filename = $"{Guid.NewGuid()}{extension}";
                var filepath = Path.Combine("Upload/Files", filename); 

                using (var stream = new FileStream(filepath, FileMode.Create))
                {
                    await i.ImageFile.CopyToAsync(stream);
                }

                i.Image = filepath;
            }

            _dbContext.Dishes.Add(i);
            await _dbContext.SaveChangesAsync();
            return await _dbContext.Dishes.ToListAsync();
        }


        public async Task<Dish> GetSingleDish(int id)
        {
            return await _dbContext.Dishes.FindAsync(id);
        }

        public async Task<Dish> UpdateDish(int id, Dish request)
        {
            Dish existingDish = await _dbContext.Dishes.FindAsync(id);
            if (existingDish != null)
            {
                existingDish.Description = request.Description;
                existingDish.Price = request.Price;
                await _dbContext.SaveChangesAsync();
            }
            return existingDish;
        }

        public async Task<List<Dish>> DeleteDish(int id)
        {
            Dish existingItem = await _dbContext.Dishes.FindAsync(id);
            if (existingItem != null)
            {
                _dbContext.Dishes.Remove(existingItem);
                await _dbContext.SaveChangesAsync();
            }
            return await _dbContext.Dishes.ToListAsync();
        }


    }
}
