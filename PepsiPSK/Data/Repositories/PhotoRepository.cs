using Microsoft.EntityFrameworkCore;
using PepsiPSK.Model;
using PSIShoppingEngine.Data;

namespace PepsiPSK.Data.Repositories
{
    public class PhotoRepository : IRepository<Photo>
    {
        private readonly DataContext _context;

        public PhotoRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Photo?> Get(Guid id)
        {
            return await _context.Photos.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Photo>> GetAll()
        {
            return await _context.Photos.ToListAsync();
        }

        public async Task<Photo> Add(Photo photo)
        {
            photo.Id = Guid.NewGuid();
            _context.Photos.Add(photo);
            await _context.SaveChangesAsync();

            return photo;
        }

        public async Task<Photo> Update(Photo photo)
        {
            _context.Photos.Update(photo);
            await _context.SaveChangesAsync();

            return photo;
        }

        public void Delete(Photo photo)
        {
            _context.Photos.Remove(photo);
            _context.SaveChanges();
        }

        public bool Exists(Guid id)
        {
            return _context.Photos.Any(p => p.Id == id);
        }
    }
}
