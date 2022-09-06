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
    public class ClassRoutineNewController : ControllerBase
    {


        private readonly IClassRoutineNewServices _classRoutineNewServices;
        public ClassRoutineNewController(IClassRoutineNewServices classRoutineNewServices)
        {
            _classRoutineNewServices = classRoutineNewServices;
        }
        [HttpGet]
        public IEnumerable<ClassRoutineNew> Gets()
        {
            return _classRoutineNewServices.Gets();
        }

        [HttpGet("{id}")]
        public IEnumerable<ClassRoutineNew> Get(int id)
        {
            return _classRoutineNewServices.Get(id);
        }
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ClassRoutineNew entity)
        {
            await _classRoutineNewServices.Create(entity);
            return Ok(entity);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<ClassRoutineNew>> Update(int id, ClassRoutineNew entity)
        {
            await _classRoutineNewServices.Update(id, entity);
            return Ok(entity);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _classRoutineNewServices.Delete(id);
            return Ok();
        }


    }
}
