using DataLayer;
using DataLayer.Entity;
using WebApp;

namespace Test
{
    public class DbTestData
    {
        public static void SeedData(FilmContext filmContext)
        {
            //SetUp
            filmContext.Films.Add(new Film() {Id = 55, Title = "James Bond"});
            filmContext.Films.Add(new Film() {Id = 65, Title = "New York Beauty"});

            filmContext.Genres.Add(new Genre() {Id = 11, Name = "Romance"});
            filmContext.Genres.Add(new Genre() {Id = 21, Name = "Action"});

            filmContext.FilmGenres.Add(new FilmGenre() {FilmId = 55, GenreId = 21});
            filmContext.FilmGenres.Add(new FilmGenre() {FilmId = 65, GenreId = 11});
            
            filmContext.SaveChanges();
        }

        public static void CleanDb(FilmContext filmContext)
        {
            //Tear Down
            filmContext.RemoveRange(filmContext.Films);
            filmContext.RemoveRange(filmContext.Genres);
            filmContext.RemoveRange(filmContext.FilmGenres);
            filmContext.RemoveRange(filmContext.FilmActors);
            filmContext.RemoveRange(filmContext.FilmDirectors);
            filmContext.RemoveRange(filmContext.FilmProducers);
            filmContext.RemoveRange(filmContext.FilmWriters);
            filmContext.RemoveRange(filmContext.FilmIdeaAuthors);
            filmContext.RemoveRange(filmContext.Participants);
            
            filmContext.SaveChanges();
        }
        
    }
}