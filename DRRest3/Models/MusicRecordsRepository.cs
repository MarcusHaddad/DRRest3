using DRRest3.Data;
using Microsoft.EntityFrameworkCore;

namespace DRRest3.Models
{
    public class MusicRecordsRepository
    {
        private readonly MusicRecordContext _context;

        public MusicRecordsRepository(MusicRecordContext context)
        {
            _context = context;
        }

        public List<MusicRecord> GetAll(string? title = null, string? artist = null)
        {
            var query = _context.MusicRecords.AsQueryable();

            if (!string.IsNullOrWhiteSpace(title))
                query = query.Where(r => r.Title.Contains(title));

            if (!string.IsNullOrWhiteSpace(artist))
                query = query.Where(r => r.Artist.Contains(artist));

            return query.ToList();
        }

        public MusicRecord? GetById(int id)
        {
            return _context.MusicRecords.Find(id);
        }

        public MusicRecord Create(MusicRecord record)
        {
            _context.MusicRecords.Add(record);
            _context.SaveChanges();
            return record;
        }

        public MusicRecord? Update(int id, MusicRecord updated)
        {
            var existing = _context.MusicRecords.Find(id);
            if (existing == null) return null;

            existing.Title = updated.Title;
            existing.Artist = updated.Artist;
            existing.Duration = updated.Duration;
            existing.Year = updated.Year;

            _context.SaveChanges();
            return existing;
        }

        public bool Delete(int id)
        {
            var record = _context.MusicRecords.Find(id);
            if (record == null) return false;

            _context.MusicRecords.Remove(record);
            _context.SaveChanges();
            return true;
        }
    }
}
