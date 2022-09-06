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
    public class GuardianController : ControllerBase
    {
        private IGuardianServices _oGuardianService;

        public GuardianController(IGuardianServices oGuardianService)
        {
            _oGuardianService = oGuardianService;
        }

        // GET: api/Guardians
        [HttpGet]
        public IEnumerable<Guardian> Get()
        {
            return _oGuardianService.Gets();
        }

        // GET: api/Guardians/5
        [HttpGet("{id}", Name = "Get116")]
        public IEnumerable<Guardian> Get(int id)
        {
            return _oGuardianService.Get(id);
        }

        // POST: api/Guardians
        [HttpPost]
        public Guardian Post([FromBody] Guardian oGuardian)
        {
            //if (ModelState.IsValid) return _oGuardianService.Save(oGuardian);
            return null;
        }

        // PUT: api/Guardians/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/Guardians/5
        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            //return _oGuardianService.Delete(id);
            return null;
        }
    }
}
