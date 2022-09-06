using ESCHOOL.Services;
using ChalkboardAPI.Models.CustomModels;
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
    public class ExamDetailsController : ControllerBase
    {


        private IExamDetailsServices _examDetailsServices;

        public ExamDetailsController(IExamDetailsServices examDetailsServices)
        {
            _examDetailsServices = examDetailsServices;
        }

        //[HttpPost("authenticate")]
        //public IActionResult Authenticate(AuthenticateRequest model)
        //{
        //    var response = _studentsService.Authenticate(model);

        //    if (response == null)
        //        return BadRequest(new { message = "Email or password is incorrect" });

        //    return Ok(response);
        //}

        [Authorize]
        [HttpGet("GetById")]
        public IActionResult GetById(int id)
        {
            var users = _examDetailsServices.GetById(id);
            return Ok(users);

        }
        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _examDetailsServices.GetAll();
            return Ok(users);

        }

        [Authorize]
        [HttpGet("GetExamDetails")]
        public IActionResult GetExamDetails(int id)
        {
            var users = _examDetailsServices.GetExamDetails(id);

            return Ok(users);
        }


        #region "Exam Details By StudentID"
        [Authorize]
        [HttpGet("GetExamDetailsByStudentId")]
        public IActionResult GetExamDetailsByStudentId(string StudentId)
        {
            try
            {
                var examType = _examDetailsServices.GetExamType(StudentId);


                var lst = new List<SectionWiseExamDetails>();

                foreach (var item in examType)
                {
                    lst.Add(new SectionWiseExamDetails()
                    {
                        ExamType = item.ExamType,
                        ExamDetailsLst = _examDetailsServices.GetExamDetailsBySectionId(item.ExamSetupID, item.SchoolId, item.SectionId)
                    });
                }
                return Ok(lst);
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }
        #endregion


        [Authorize]
        [HttpGet("GetExamTaskDetails")]
        public async Task<IActionResult> GetExamTaskDetails(int StudentId)
        {
            return Ok(await _examDetailsServices.GetExamTaskDetails(StudentId));
        }

        [Authorize]
        [HttpGet("GetTestDetails")]
        public async Task<IActionResult> GetTestDetails(int studentId, int subjectid)
        {
            return Ok(await _examDetailsServices.GetTestDetails(studentId, subjectid));
        }

    }
}
