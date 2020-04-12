using System.Collections.Generic;
using DataLayer.Entity;

namespace WebApp.ViewModels.Film
{
    public class FilmDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        
        public List<FilmGenre> FilmGenres { get; set; }
    }
}