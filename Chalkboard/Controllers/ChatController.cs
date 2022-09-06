using ChalkboardAPI.Models;
using ESCHOOL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESCHOOL.Models;

namespace ESCHOOL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {

        private IChatServices _chatServices;

        public ChatController(IChatServices chatServices)
        {
            _chatServices = chatServices;
        }

        //[HttpPost("authenticate")]
        //public IActionResult Authenticate(AuthenticateRequest model)
        //{
        //    var response = _chatServices.Authenticate(model);

        //    if (response == null)
        //        return BadRequest(new { message = "Email or password is incorrect" });

        //    return Ok(response);
        //} 
        [Authorize]
        [HttpPost("Insert")]
        public IActionResult ChatMessage(Chat ct)
        {
            //Chat ct=new Chat();
            var chatResult=_chatServices.SaveAll(ct);
            if (chatResult == null)
                return BadRequest(new {message = "Passing value Can not correct format"});
            return Ok(chatResult);
        }
        [Authorize]
        [HttpGet("GetById")]
        public IActionResult GetById(int id)
        {
            var users = _chatServices.GetById(id);
            return Ok(users);

        }
        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _chatServices.GetAll();
            return Ok(users);

        }

        [Authorize]
        [HttpPut("Update")]
        public IActionResult UpdateChat(Chat ct)
        {
            var response = _chatServices.updateChat(ct);
            return Ok(response);
        }

        [Authorize]
        [HttpDelete("Delete")]
        public IActionResult DeleteChat(int id)
        {
            var response = _chatServices.deleteChat(id);
            return Ok(response);
        }
    }
}
