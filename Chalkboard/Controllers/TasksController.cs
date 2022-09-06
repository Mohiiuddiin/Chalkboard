using ChalkboardAPI.Models.CustomModels;
using ESCHOOL.Models;
using ESCHOOL.Services;
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
    public class TasksController : ControllerBase
    {
        private ITasksServices _tasksServices;

        public TasksController(ITasksServices tasksServices)
        {
            _tasksServices = tasksServices;
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
            var users = _tasksServices.GetById(id);
            return Ok(users);

        }

        //#region "Task List By StudentID"
        //[Authorize]
        //[HttpGet("GetSubjectWiseTaskListByStudentId")]
        //public IActionResult GetSubjectWiseTaskListByStudentId(string StudentId)
        //{

        //    //DotNetCompilerPlatform
        //    try
        //    {
        //        var subjects = _tasksServices.GetSubjectsByStudentId(StudentId);


        //        var lst = new List<SubjectWiseTasks>();

        //        foreach (var item in subjects)
        //        {
        //            lst.Add(
        //                new SubjectWiseTasks()
        //                {
        //                    SubjectName = item.SubjectName,
        //                    TasktLst = _tasksServices.GetTaskListBySubjectId(item.SubjecId)
        //                }
        //                );
        //        }
        //        return Ok(lst);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest();
        //    }
    
        //}
        //#endregion

        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _tasksServices.GetAll();
            return Ok(users);

        }
    }
}
