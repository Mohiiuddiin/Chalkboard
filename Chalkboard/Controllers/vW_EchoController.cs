using ChalkboardAPI.Models;
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
    public class vW_EchoController : ControllerBase
    {


        private IvW_EchoServices _ovW_EchoServices;

        public vW_EchoController(IvW_EchoServices ovW_EchoServices)
        {
            _ovW_EchoServices = ovW_EchoServices;
        }

        // GET: api/ Examss
        [HttpGet]
        public IEnumerable<vW_Echo> Get()
        {
            return _ovW_EchoServices.Gets();
        }

        // GET: api/ Examss/5
        [HttpGet("{id}")]
        public IEnumerable<vW_Echo> Get(int id)
        {
            return _ovW_EchoServices.Get(id);
        }

        // POST: api/ Examss
        [HttpPost]
        public vW_Echo Post([FromBody] vW_Echo oExams)
        {
            //if (ModelState.IsValid) return _o ExamsService.Save(o Exams);
            return null;
        }

        // PUT: api/ Examss/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ Examss/5
        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            //return _o ExamsService.Delete(id);
            return null;
        }
    }
}
