using DB;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        private HotelContext _context;

        public RoomsController(HotelContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IEnumerable<rooms> Get() {
           //var roomDB = _context.Rooms.Where(room => room.status == true).ToList();
            var roomDB = _context.Rooms.ToList();

            foreach (var room in roomDB)
            {
                room.room_identity = room.room_identity.ToString().Trim();
                room.room_type = room.room_type.ToString().Trim();
            }

            return roomDB;
            
        } 
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] rooms rooms)
        {
            _context.Rooms.Add(rooms);
            await _context.SaveChangesAsync();

            return new OkObjectResult(rooms);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (id < 0)
            {
                return new NotFoundResult();
            }
            var roomDB = await _context.Rooms.FindAsync(id);
            if (roomDB == null)
            {
                return new NotFoundResult();
            }
            roomDB.room_identity = roomDB.room_identity.ToString().Trim();
            roomDB.room_type = roomDB.room_type.ToString().Trim();
            return new OkObjectResult(roomDB);

        }


        [HttpPut("{id}")]

        public async Task<IActionResult> Put(int id, [FromBody] rooms rooms)
        {
            if (id != rooms.id_room)
            {
                return new BadRequestResult();
            }
            var roomDB = await _context.Rooms.FindAsync(id);
            if (roomDB == null)
            {
                return new NotFoundResult();
            }

            roomDB.room_identity = rooms.room_identity;
            roomDB.room_type = rooms.room_type;
            roomDB.bedroom_numbers = rooms.bedroom_numbers;
            roomDB.bed_numbers = rooms.bedroom_numbers;
            roomDB.number_bathrooms = rooms.number_bathrooms;
            roomDB.status = rooms.status;

            await _context.SaveChangesAsync();
            return new OkObjectResult(rooms);

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var roomDB = await _context.Rooms.FindAsync(id);
            if (roomDB == null)
            {
                return new NotFoundResult();
            }

            roomDB.status = false;
            await _context.SaveChangesAsync();

            return new OkObjectResult(id);
        }

    }
}
