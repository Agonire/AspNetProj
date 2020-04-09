using DataLayer.Contract;
using DataLayer.Entity;

namespace DataLayer.Repository
{
    public class GenreRepo: RepositoryBase<Genre>, IGenreRepo
    {
        public GenreRepo(FilmContext context) : base(context)
        {
        }
    }
}