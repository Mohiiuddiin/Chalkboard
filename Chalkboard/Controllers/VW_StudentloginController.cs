using ChalkboardAPI.Services;
using ChalkboardAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESCHOOL.Models;

namespace ChalkboardAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VW_StudentloginController : ControllerBase
    {
        
        private IVW_StudentLoginServicese _studentloginServicese;

        public VW_StudentloginController(IVW_StudentLoginServicese studentloginServicese)
        {
            _studentloginServicese = studentloginServicese;
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticateRequest model)
        {   
            try
            {
                VW_StudentLogin stdmodel = new VW_StudentLogin();

                var response = _studentloginServicese.Authenticate(model);

                if (stdmodel.LoginActive == 2)
                {
                    return BadRequest(new { message = "You are not authorized to access" });
                }
                else
                {
                    if (response == null)
                        return BadRequest(new { message = "Email or password is incorrect" });
                }
                return Ok(response);
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        [Authorize]
        [HttpPost("SaveLoginHistory")]
        public IActionResult SaveLoginHistory(LoginHistory loginHistory)
        {
            try
            {
                int rowAffected = _studentloginServicese.InsertLoginHistory(loginHistory);

                if (loginHistory == null)
                {
                    return BadRequest(new { message = "Object Cant be null" });
                }
                if (string.IsNullOrEmpty(loginHistory.MemberID))
                {
                    return BadRequest(new { message = "MembrerId Required" });
                }

                if (rowAffected == 0)
                {
                    return BadRequest(new { message = "Login History Record Failed" });
                }
                else if(rowAffected==1)
                {
                    return Ok("Save Successfull");
                }
                return BadRequest(new { message = "404" });
            }
            catch (Exception)
            {
                throw;
            }

        }


        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _studentloginServicese.GetAll();
            return Ok(users);
        }

        [Authorize]
        [HttpPut("getdeviceid")]
        public async Task<ActionResult<VW_StudentLogin>> GetDeviceInfo([FromBody] VW_StudentLogin entity)
        {
            await _studentloginServicese.UpdateDeviceId(entity);
            return Ok(entity);
        }

        [Authorize]
        [HttpPut("removestddeviceid")]
        public async Task<ActionResult<VW_StudentLogin>> RemoveStdDeviceId([FromBody] VW_StudentLogin entity)
        {
            await _studentloginServicese.RemoveStdDeviceId(entity);
            return Ok(entity);
        }
    }
}
