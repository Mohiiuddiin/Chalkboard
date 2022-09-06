using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ESCHOOL.IServices;
using ESCHOOL.Models;

namespace ESCHOOL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private IPhotosServices _oPhotosService;

        public PhotosController(IPhotosServices oPhotosService)
        {
            _oPhotosService = oPhotosService;
        }

        // GET: api/Photoss
        [HttpGet]
        public IEnumerable<Photos> Get()
        {
            return _oPhotosService.Gets();
        }

        // GET: api/Photoss/5
        [HttpGet("{id}", Name = "Get117")]
        public IEnumerable<Photos> Get(int id)
        {
            return _oPhotosService.Get(id);
        }

        // POST: api/Photoss
        [HttpPost]
        public Photos Post([FromBody] Photos oPhotos)
        {
            //if (ModelState.IsValid) return _oPhotosService.Save(oPhotos);
            return null;
        }

        // PUT: api/Photoss/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/Photoss/5
        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            //return _oPhotosService.Delete(id);
            return null;
        }
    }
}
