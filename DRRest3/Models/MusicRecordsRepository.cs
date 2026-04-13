namespace DRRest3.Models
{
    public class MusicRecordsRepository
    {
        private static List<MusicRecord> _musicRecords = new List<MusicRecord>();
        
        private static int _nextId = 1;
        public List<MusicRecord> GetAll()
        {
            return new List<MusicRecord>(_musicRecords);
        }
        public MusicRecord? GetByTitle(string Title)
        {
            return _musicRecords.FirstOrDefault(m => m.Title == Title);
        }


    }
}
