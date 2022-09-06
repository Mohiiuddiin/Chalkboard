//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using ESCHOOL.IServices;
//using ESCHOOL.Models;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace ESCHOOL.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class vw_Bravo1Controller : ControllerBase
//    {
//        private Ivw_BravoService _vw_Bravo1Service;
//        public vw_Bravo1Controller(Ivw_BravoService oStudentService)
//        {
//            _vw_Bravo1Service = oStudentService;
//        }
//        // GET: api/vw_Bravo1
//        [HttpGet]
//        public IEnumerable<vw_Bravo> Get()
//        {
//            return _vw_Bravo1Service.Gets();
//        }
//        // GET: api/vw_Bravo1/5
//        [HttpGet("{id}", Name = "GetResult1")]
//        public IEnumerable<vw_Bravo> Get(int id)
//        {
//            return _vw_Bravo1Service.Get(id);
//        }


//        // POST: api/vw_Bravo1
//        [HttpPost]
//        public void Post([FromBody] string value)
//        {
//        }

//        // PUT: api/vw_Bravo1/5
//        [HttpPut("{id}")]
//        public void Put(int id, [FromBody] string value)
//        {
//        }

//        // DELETE: api/ApiWithActions/5
//        [HttpDelete("{id}")]
//        public void Delete(int id)
//        {
//        }
//    }
//}
