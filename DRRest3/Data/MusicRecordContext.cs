using DRRest3.Models;
using Microsoft.EntityFrameworkCore;

namespace DRRest3.Data
{
    public class MusicRecordContext : DbContext
    {
        public MusicRecordContext(DbContextOptions<MusicRecordContext> options) : base(options) { }

        public DbSet<MusicRecord> MusicRecords { get; set; }
    }
}
