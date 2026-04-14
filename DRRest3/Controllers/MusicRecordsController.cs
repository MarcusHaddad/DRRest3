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
        [HttpGet]
        public ActionResult<IEnumerable<MusicRecord>> Get([FromQuery] string? title = null, [FromQuery] string? artist = null)
        {
            return Ok(_repository.GetAll(title, artist));
        }

        // GET api/musicrecords/5
        [HttpGet("{id}")]
        public ActionResult<MusicRecord> Get(int id)
        {
            var record = _repository.GetById(id);
            if (record == null) return NotFound();
            return Ok(record);
        }

        // POST api/musicrecords
        [Authorize]
        [HttpPost]
        public ActionResult<MusicRecord> Post([FromBody] MusicRecord record)
        {
            var created = _repository.Create(record);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        // PUT api/musicrecords/5
        [Authorize]
        [HttpPut("{id}")]
        public ActionResult<MusicRecord> Put(int id, [FromBody] MusicRecord record)
        {
            var updated = _repository.Update(id, record);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        // DELETE api/musicrecords/5
        [Authorize]
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            if (!_repository.Delete(id)) return NotFound();
            return NoContent();
        }
    }
}
