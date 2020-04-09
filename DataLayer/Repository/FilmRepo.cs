using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DataLayer.Contract;
using DataLayer.Entity;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repository
{
    public class FilmRepo : RepositoryBase<Film>, IFilmRepo
    {
        public FilmRepo(FilmContext context) : base(context)
        {
        }

        public void CascadeUpdate(Film oldFilm, Film newFilm)
        {
            oldFilm.Title = newFilm.Title;

            var genresToAdd = newFilm.FilmGenres.Except(oldFilm.FilmGenres).ToList();
            var genresToRemove = oldFilm.FilmGenres.Except(newFilm.FilmGenres).ToList();

            genresToRemove.ForEach(filmGenre => Context.Set<FilmGenre>().Remove(filmGenre));
            genresToAdd.ForEach(filmGenre => Context.Set<FilmGenre>().Add(filmGenre));
            
            Update(oldFilm);
        }

        public new IQueryable<Film> Find(Expression<Func<Film, bool>> expression)
        {
            return Context.Set<Film>().Where(expression)
                .Include(f => f.FilmGenres)
                .ThenInclude(fg => fg.Genre)
                .Include(f => f.FilmActors)
                .ThenInclude(fa => fa.Participant)
                .AsNoTracking();
        }
    }
}