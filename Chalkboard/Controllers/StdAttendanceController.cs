using ESCHOOL.Models;
using Chalkboard.Models.CustomModels;
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
    public class StdAttendanceController : ControllerBase
    {
        private IStdAttendanceServices _stdAttendanceServices;

        public StdAttendanceController(IStdAttendanceServices stdAttendanceServices)
        {
            _stdAttendanceServices = stdAttendanceServices;
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
        [HttpPost("SaveAttendance")]
        public IActionResult SaveAttendance(StdAttendance attendance)
        {
            try
            {
                int rowAffected = _stdAttendanceServices.InsertAttendance(attendance);

                if (attendance == null)
                {
                    return BadRequest(new { message = "Object Cant be null" });
                }
                if (string.IsNullOrEmpty(attendance.StdId))
                {
                    return BadRequest(new { message = "Student Id Required" });
                }

                if (rowAffected == 0)
                {
                    return BadRequest(new { message = "Failed" });
                }
                else if (rowAffected == 1)
                {
                    return Ok("Save Successfull");
                }
                else if (rowAffected == -1)
                {
                    return Ok("already checked in");
                }
                else if (rowAffected == -2)
                {
                    return Ok("already checked out");
                }
                else if (rowAffected == -3)
                {
                    return Ok("please check in first");
                }
                return BadRequest(new { message = "404" });
            }
            catch (Exception)
            {
                throw;
            }

        }


        [Authorize]
        [HttpGet("GetStudentAttendance")]
        public IActionResult GetStudentAttendance(int stdId,DateTime? fromDate,DateTime? toDate,int year,int month)
        {
            try
            {
                if (stdId==0)
                {                    
                    return Ok("STUDENT ID REQUIRED");
                }
                else
                {                    
                    //if (fromDate!=null && toDate!=null)
                    //{
                        var data = _stdAttendanceServices.GetAttendanceByStdId(stdId,fromDate,toDate,year,month);
                        return Ok(data);
                    //}
                    //else
                    //{
                    //    var data = _stdAttendanceServices.GetAttendanceByStdId(stdId,null,null);
                    //    return Ok(data);
                    //}
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
        [Authorize]
        [HttpGet("GetById")]
        public IActionResult GetById(int id)
        {
            var users = _stdAttendanceServices.GetById(id);
            return Ok(users);
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _stdAttendanceServices.GetAll();
            return Ok(users);
        }
    }
}
