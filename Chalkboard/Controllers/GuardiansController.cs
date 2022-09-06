using ESCHOOL.IServices;
using ESCHOOL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESCHOOL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuardiansController : ControllerBase
    {



        private readonly IGuardiansServices _guardiansServices;
        public GuardiansController(IGuardiansServices guardiansServices)
        {
            _guardiansServices = guardiansServices;
        }
        [HttpGet]
        public IEnumerable<Guardians> Gets()
        {
            return _guardiansServices.Gets();
        }

        [HttpGet("{id}")]
        public IEnumerable<Guardians> Get(int id)
        {
            return _guardiansServices.Get(id);
        }
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Guardians entity)
        {
            await _guardiansServices.Create(entity);
            return Ok(entity);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<Guardians>> Update(int id, Guardians entity)
        {
            await _guardiansServices.Update(id, entity);
            return Ok(entity);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _guardiansServices.Delete(id);
            return Ok();
        }

    }
}
