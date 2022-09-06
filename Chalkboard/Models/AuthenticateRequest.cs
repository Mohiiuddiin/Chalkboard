using System.ComponentModel.DataAnnotations;

namespace ChalkboardAPI.Models
{
    public class AuthenticateRequest
    {
        [Required]
        public string Email { get; set; }

        //[Required]
        //public string MobileNo { get; set; }
        [Required]
        public string Password { get; set; }

    }
}