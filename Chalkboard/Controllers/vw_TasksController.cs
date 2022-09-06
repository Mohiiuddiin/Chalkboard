using ChalkboardAPI.Models.CustomModels;
using ChalkboardAPI.IServices;
using ESCHOOL.IServices;
using ChalkboardAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChalkboardAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class vw_TasksController : ControllerBase
    {

        private Ivw_TasksServices _ovW_TasksServices;

        public vw_TasksController(Ivw_TasksServices ovW_EchoServices)
        {
            _ovW_TasksServices = ovW_EchoServices;
        }

        // GET: api/ Examss
        [HttpGet]
        public IEnumerable<vw_Tasks> Get()
        {
            return _ovW_TasksServices.Gets();
        }

        // GET: api/ Examss/5
        [HttpGet("{id}")]
        public IEnumerable<vw_Tasks> Get(int id)
        {
            return _ovW_TasksServices.Get(id);
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

        #region "Task List By StudentID"
        [Authorize]
        [HttpGet("GetSubjectWiseTaskListByStudentId")]
        public IActionResult GetSubjectWiseTaskListByStudentId(string StudentId)
        {

            //DotNetCompilerPlatform
            try
            {
                var subjects = _ovW_TasksServices.GetSubjectsByStudentId(StudentId);


                var lst = new List<SubjectWiseTasks>();

                foreach (var item in subjects)
                {
                    lst.Add(new SubjectWiseTasks()
                        {
                            SubjectName = item.SubjectName,
                            TasktLst = _ovW_TasksServices.GetTaskListBySubjectId(item.SubjecId)
                        });
                }
                return Ok(lst);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }
        #endregion

        #region "Task Result"
        [Authorize]
        [HttpGet("GetTaskResultByStudentId")]
        public IActionResult GetTaskResultByStudentId(string StudentId)
        {

            //DotNetCompilerPlatform
            try
            {
                var subjects = _ovW_TasksServices.GetSubjectsTestResultByStudentId(StudentId);

                return Ok(subjects);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }
        #endregion


    }
}
