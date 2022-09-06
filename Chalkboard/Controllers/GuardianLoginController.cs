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
    public class GuardianLoginController : ControllerBase
    {

        private IVW_StudentLoginServicese _studentloginServicese;

        public GuardianLoginController(IVW_StudentLoginServicese studentloginServicese)
        {
            _studentloginServicese = studentloginServicese;
        }

        [HttpPost("authenticate")]
        public IActionResult GuardianAuthenticate(AuthenticateRequest model)
        {
            VW_StudentLogin stdmodel = new VW_StudentLogin();

            var response = _studentloginServicese.GuardianAuthenticate(model);

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

        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _studentloginServicese.GetAll();
            return Ok(users);
        }

        [Authorize]
        [HttpPut("getguardiandeviceid")]
        public async Task<ActionResult<VW_StudentLogin>> GetGuardianDeviceInfo([FromBody] VW_StudentLogin entity)
        {
            await _studentloginServicese.UpdateGuardianDeviceId(entity);
            return Ok(entity);
        }


        [Authorize]
        [HttpPut("removeguardiandeviceid")]
        public async Task<ActionResult<VW_StudentLogin>> RemoveGuardianDeviceId([FromBody] VW_StudentLogin entity)
        {
            await _studentloginServicese.RemoveGuardianDeviceId(entity);
            return Ok(entity);
        }

    }
}
