using DRRest3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DRRest3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicRecordsController : ControllerBase
    {
        private readonly MusicRecordsRepository _repository;

        public MusicRecordsController(MusicRecordsRepository repository)
        {
            _repository = repository;
        }

        // GET: api/musicrecords?title=...&artist=...
        // 200 OK - returnerer liste (kan være tom)
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<MusicRecord>> Get([FromQuery] string? title = null, [FromQuery] string? artist = null)
        {
            return Ok(_repository.GetAll(title, artist));
        }

        // GET api/musicrecords/5
        // 200 OK - fundet
        // 404 Not Found - findes ikke
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<MusicRecord> Get(int id)
        {
            var record = _repository.GetById(id);
            if (record == null) return NotFound();
            return Ok(record);
        }

        // POST api/musicrecords
        // 201 Created - oprettet
        // 400 Bad Request - ugyldigt input
        // 401 Unauthorized - ikke logget ind
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<MusicRecord> Post([FromBody] MusicRecord record)
        {
            if (string.IsNullOrWhiteSpace(record.Title) || string.IsNullOrWhiteSpace(record.Artist))
                return BadRequest("Titel og artist må ikke være tomme.");

            var created = _repository.Create(record);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        // PUT api/musicrecords/5
        // 200 OK - opdateret
        // 400 Bad Request - ugyldigt input
        // 401 Unauthorized - ikke logget ind
        // 404 Not Found - findes ikke
        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<MusicRecord> Put(int id, [FromBody] MusicRecord record)
        {
            if (string.IsNullOrWhiteSpace(record.Title) || string.IsNullOrWhiteSpace(record.Artist))
                return BadRequest("Titel og artist må ikke være tomme.");

            var updated = _repository.Update(id, record);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        // DELETE api/musicrecords/5
        // 204 No Content - slettet
        // 401 Unauthorized - ikke logget ind
        // 404 Not Found - findes ikke
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Delete(int id)
        {
            if (!_repository.Delete(id)) return NotFound();
            return NoContent();
        }
    }
}
