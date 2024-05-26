using DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;
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
        // Metodo para obtener todas los Huespedes
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
        // Metodo para crear un huesped
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
            // creamos el registro del huesped registrado
            var newRecord = new records
            {
                record_fullname = guest.guest_fullname,
                record_dni = guest.guest_dni,
                record_phone_number = guest.guest_phone_number,
                record_admission_date = guest.admission_date,
                record_departure_date = guest.departure_date,
                record_room = room.room_identity,
                id_guest = guest.id_guest,
                id_room = guest.id_room,
            };
            _context.Records.Add(newRecord);
            await _context.SaveChangesAsync();

            return new OkObjectResult(guest);

        }
        // Metodo para traer una huesped por medio de su id
        [HttpGet("{id}")]

        public async Task<IActionResult> GetGuest(int id)
        {
            if (id < 0)
            {
                return new NotFoundResult();
            }
            var GuestDB = await _context.Guest.FindAsync(id);
            if (GuestDB == null)
            {
                return new NotFoundResult();
            }
            GuestDB.guest_fullname = GuestDB.guest_fullname.ToString().Trim();
            return new OkObjectResult(GuestDB);
        }
        // Metodo paraactualizar un huesped por medio de su id
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
        // Metodo para eliminar un huesped por medio de su id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGuest(int id)
        {
            var GuestDB = await _context.Guest.FindAsync(id);
            if (GuestDB == null)
            {
                return new NotFoundResult();
            }
            // obtener la habitacion del huesped
            var room = await _context.Rooms.FindAsync(GuestDB.id_room);
            if (room.status == false)
            {
               room.status = true; // colocarla nuevamente activa
            }
            _context.Guest.Remove(GuestDB);
   
            await _context.SaveChangesAsync();

            return new OkObjectResult(id);
        }

    }
}
