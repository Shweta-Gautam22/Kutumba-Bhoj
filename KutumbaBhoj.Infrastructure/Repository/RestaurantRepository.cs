using KutumbaBhoj.Application.Interfaces;
using KutumbaBhoj.Domain.Models;
using KutumbaBhoj.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace KutumbaBhoj.Infrastructure.Repository
{
    public class RestaurantRepository:IRestaurantRepository
    {
        private readonly DataContext _dbContext;

        public RestaurantRepository(DataContext DbContext)
        {
            _dbContext = DbContext;
        }
        public async Task<List<Restaurant>> GetAllRestaurants()
        {
            return await _dbContext.Restaurants.ToListAsync();
        }

        public async Task<List<Restaurant>> AddRestaurants(Restaurant i)
        {
            if (i.LogoFile != null && i.LogoFile.Length > 0)
            {
                var extension = Path.GetExtension(i.LogoFile.FileName);
                var filename = $"{Guid.NewGuid()}{extension}";
                var filepath = Path.Combine("Upload/Files", filename);

                using (var stream = new FileStream(filepath, FileMode.Create))
                {
                    await i.LogoFile.CopyToAsync(stream);
                }

                i.Logo = filepath;
            }

            _dbContext.Restaurants.Add(i);
            await _dbContext.SaveChangesAsync();
            return await _dbContext.Restaurants.ToListAsync();
        }

        public async Task<Restaurant> GetSingleRestaurant(int id)
        {
            return await _dbContext.Restaurants.FindAsync(id);
        }

        public async Task<Restaurant> UpdateRestaurant(int id, Restaurant request)
        {
            Restaurant existingRestaurant = await _dbContext.Restaurants.FindAsync(id);
            if (existingRestaurant != null)
            {
                existingRestaurant.RestaurantName = request.RestaurantName;
                await _dbContext.SaveChangesAsync();
            }
            return existingRestaurant;
        }

        public async Task<List<Restaurant>> DeleteRestaurant(int id)
        {
            Restaurant existingRestaurant = await _dbContext.Restaurants.FindAsync(id);
            if (existingRestaurant != null)
            {
                _dbContext.Restaurants.Remove(existingRestaurant);
                await _dbContext.SaveChangesAsync();
            }
            return await _dbContext.Restaurants.ToListAsync();
        }



    }
}
