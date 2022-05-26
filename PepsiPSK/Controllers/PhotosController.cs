#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PepsiPSK.Entities;
using PepsiPSK.Models.Photos;
using PepsiPSK.Services.Photos;

namespace PepsiPSK.Controllers.Photos
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IPhotoService _service;

        public PhotosController(IPhotoService service)
        {
            _service = service;
        }

        // GET: api/Photos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PhotoListDto>>> GetPhotos()
        {
            return await _service.GetAll();
        }

        // GET: api/Photos/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetPhoto(Guid id)
        {
            var photo = await _service.Get(id);

            if (photo == null)
            {
                return NotFound();
            }
            else
            {
                var imageData = Convert.FromBase64String(photo.Content);
                return File(imageData, photo.ContentType);
            }
        }

        // POST: api/Photos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Photo>> PostPhoto(IFormFile image)
        {
            var photo = await _service.Add(image);
            return photo == null ? BadRequest() : Ok(photo);
        }

        // DELETE: api/Photos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(Guid id)
        {
            var photo = await _service.Get(id);
            if (photo == null)
            {
                return NotFound();
            }

            _service.Delete(photo);

            return Ok();
        }
    }
}
