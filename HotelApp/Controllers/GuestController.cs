using DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlTypes;
using System.Globalization;

namespace HotelApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GuestController : ControllerBase
    {
        private HotelContext _context;

        public GuestController(HotelContext context)
        {
            _context = context;

        }
        [HttpGet]
        public IEnumerable<guest> GetGuests()
        {
            var GuestDB = _context.Guest.ToList();

            foreach (var guest in GuestDB)
            {
                guest.guest_fullname = guest.guest_fullname.ToString().Trim();
            }
            return GuestDB;
        }
        [HttpPost]
        public async Task<IActionResult> PostGuests([FromBody] guest guest)
        {
            if (guest.admission_date < SqlDateTime.MinValue.Value || guest.admission_date > SqlDateTime.MaxValue.Value)
            {
                return BadRequest("La fecha de admisión está fuera del rango válido.");
            }

            var room = await _context.Rooms.FindAsync(guest.id_room);
            if (room == null)
            {
                return NotFound("La habitación no existe");
            }
            if (!room.status)
            {
                return BadRequest("La habitación está ocupada");
            }
            room.status = false;

            _context.Guest.Add(guest);
            await _context.SaveChangesAsync();

            return new OkObjectResult(guest);

        }
        [HttpGet("{id}")]

        public async Task<IActionResult> GetGuest(int id)
        {
            if(id < 0)
            {
                return new NotFoundResult();
            }
            var GuestDB = await _context.Guest.FindAsync(id);
            if(GuestDB == null)
            {
                return new NotFoundResult();
            }
            GuestDB.guest_fullname = GuestDB.guest_fullname.ToString().Trim();
            return new OkObjectResult(GuestDB);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGuest(int id, [FromBody] guest guest)
        {
            if (id != guest.id_guest)
            {
                return new BadRequestResult();
            }
            var GuestDB = await _context.Guest.FindAsync(id);
            if (GuestDB == null)
            {
                return new NotFoundResult();
            }

            // Obtener la habitación anterior del huésped
            var previousRoom = await _context.Rooms.FindAsync(GuestDB.id_room);
            if (previousRoom != null)
            {
                // Marcar la habitación anterior como disponible
                previousRoom.status = true;
            }

            GuestDB.guest_fullname = guest.guest_fullname;
            GuestDB.guest_dni = guest.guest_dni;
            GuestDB.guest_phone_number = guest.guest_phone_number;
            GuestDB.admission_date = guest.admission_date;
            GuestDB.departure_date = guest.departure_date;
            GuestDB.id_room = guest.id_room;

            // Obtener la nueva habitación del huésped
            var newRoom = await _context.Rooms.FindAsync(GuestDB.id_room);
            if (newRoom != null)
            {
                // Marcar la nueva habitación como ocupada
                newRoom.status = false;
            }

            await _context.SaveChangesAsync();
            return new OkObjectResult(guest);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGuest(int id)
        {
            var GuestDB = await _context.Guest.FindAsync(id);
            if (GuestDB == null)
            {
                return new NotFoundResult();
            }
            var room = await _context.Rooms.FindAsync(GuestDB.id_room);
            if (room.status == false)
            {
               room.status = true;
            }
            _context.Guest.Remove(GuestDB);
            
            await _context.SaveChangesAsync();

            return new OkObjectResult(id);
        }

    }
}
