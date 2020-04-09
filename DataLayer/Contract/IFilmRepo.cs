using System.Linq;
using DataLayer.Entity;

namespace DataLayer.Contract
{
    public interface IFilmRepo : IRepositoryBase<Film>
    {
        public void CascadeUpdate(Film oldFilm, Film newFilm);
    }
}