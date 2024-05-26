using DB;
using Microsoft.AspNetCore.Mvc;

namespace HotelApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecordsController : ControllerBase
    {
       private HotelContext _context;

        public RecordsController(HotelContext context)
        {
            _context = context;
        }
        // Metodo para obtener todas los registros
        [HttpGet]
        public IEnumerable<records> GetRecords()
        {
            return _context.Records.ToList();
        }
        // Metodo para eliminar un registro por medio de su id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGuest(int id)
        {
            var RecordDB = await _context.Records.FindAsync(id);
            if (RecordDB == null)
            {
                return new NotFoundResult();
            }
            _context.Records.Remove(RecordDB);

            await _context.SaveChangesAsync();

            return new OkObjectResult(id);
        }
    }
}
