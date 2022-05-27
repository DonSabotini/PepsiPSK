using PepsiPSK.Entities;
using PepsiPSK.Models.Photos;

namespace PepsiPSK.Services.Photos
{
    public interface IPhotoService
    {
        Task<Photo?> Get(Guid id);
        Task<List<PhotoListDto>> GetAll();
        Task<Photo?> Add(IFormFile image);
        void Delete(Photo photo);
        bool Exists(Guid id);
    }
}
