namespace DataLayer.Entity
{
    public class FilmWriter
    {
        public int FilmId { get; set; }

        public int ParticipantId { get; set; }

        public Film Film { get; set; }

        public Participant Participant { get; set; }
    }
}