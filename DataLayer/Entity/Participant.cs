using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Entity
{
    public class Participant
    {
        [DatabaseGenerated((DatabaseGeneratedOption.Identity))]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }
        
        public string MiddleName { get; set; }
        
        [Required]
        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }

        
        public ICollection<FilmGenre> FilmGenres { get; set; }
        
        public ICollection<FilmDirector> FilmDirectors { get; set; }
        
        public ICollection<FilmProducer> FilmProducers { get; set; }
        
        public ICollection<FilmIdeaAuthor> FilmIdeaAuthors { get; set; }
        
        public ICollection<FilmWriter> FilmWriters { get; set; }
        
        public ICollection<FilmActor> FilmActors { get; set; }
    }
}