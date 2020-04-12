using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Entity
{
    public class Film
    {
        public Film()
        {
            FilmGenres = new List<FilmGenre>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Title { get; set; }

        
        public ICollection<FilmGenre> FilmGenres { get; set; }
        
        public ICollection<FilmDirector> FilmDirectors { get; set; }
        
        public ICollection<FilmProducer> FilmProducers { get; set; }
        
        public ICollection<FilmIdeaAuthor> FilmIdeaAuthors { get; set; }
        
        public ICollection<FilmWriter> FilmWriters { get; set; }
        
        public ICollection<FilmActor> FilmActors { get; set; }
    }
}