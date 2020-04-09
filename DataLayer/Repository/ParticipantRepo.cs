using DataLayer.Contract;
using DataLayer.Entity;

namespace DataLayer.Repository
{
    public class ParticipantRepo: RepositoryBase<Participant>, IParticipantRepo
    {
        public ParticipantRepo(FilmContext context) : base(context)
        {
        }
    }
}