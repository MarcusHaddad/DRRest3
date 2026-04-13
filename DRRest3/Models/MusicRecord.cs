namespace DRRest3.Models
{
    public class MusicRecord
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Artist { get; set; } = string.Empty;
        public int Duration { get; set; } // Duration in seconds
        public int Year { get; set; }
    }
}
