using DB;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;

namespace HotelApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private HotelContext _context;

        public LoginController(HotelContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] login login)
        {
            var user = _context.Login.FirstOrDefault(u => u.email == login.email);

            if (user == null)
            {
                return BadRequest("email does not exist");
            }
            //Borrado de espacios sobrantes
            user.email = user.email.ToString().Trim();
            user.password = user.password.ToString().Trim();

            if (user.password == login.password) {

                return Ok(user.email);

            }else
            {
                return Unauthorized();
            }

        }
    }
}
