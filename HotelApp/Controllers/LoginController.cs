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
        [HttpPost("validate")]
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
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string email)
        {
            var user = _context.Login.FirstOrDefault(u => u.email == email);

            if (user == null)
            {
                return BadRequest("User not found");
            }

            return Ok(user.id_rol);
        }
        [HttpPost("register")]
        public async Task<IActionResult> PostL([FromBody] login login)
        {
            _context.Login.Add(login);
            await _context.SaveChangesAsync();

            return new OkObjectResult(login);
        }
        [HttpGet("users")]
        public IEnumerable<login> GetU()
        {
            var usersDB = _context.Login.ToList();

            foreach (var user in usersDB)
            {
                user.password = user.password.ToString().Trim();
  
            }

            return usersDB;
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteU(int id)
        {
            var UsersDB = await _context.Login.FindAsync(id);
            if (UsersDB == null)
            {
                return new NotFoundResult();
            }
            _context.Login.Remove(UsersDB);

            await _context.SaveChangesAsync();

            return new OkObjectResult(id);
        }
        [HttpPut("{id}")]

        public async Task<IActionResult> Put(int id, [FromBody] login login)
        {
            if (id != login.id)
            {
                return new BadRequestResult();
            }
            var userDB = await _context.Login.FindAsync(id);
            if (userDB == null)
            {
                return new NotFoundResult();
            }

            userDB.email = login.email;
            userDB.password = login.password;
            userDB.id_rol = login.id_rol;

            await _context.SaveChangesAsync();
            return new OkObjectResult(login);

        }
    }
}
