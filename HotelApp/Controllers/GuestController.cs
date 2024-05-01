﻿using DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            if(id != guest.id_guest)
            {
                return new BadRequestResult();
            }
            var GuestDB = await _context.Guest.FindAsync(id);
            if (GuestDB == null)
            {
                return new NotFoundResult();
            }

            GuestDB.guest_fullname = guest.guest_fullname;
            GuestDB.guest_dni = guest.guest_dni;
            GuestDB.guest_phone_number = guest.guest_phone_number;
            GuestDB.admission_date = guest.admission_date;
            GuestDB.departure_date = guest.departure_date;
            GuestDB.id_room = guest.id_room;

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

            
            await _context.SaveChangesAsync();

            return new OkObjectResult(id);
        }

    }
}
