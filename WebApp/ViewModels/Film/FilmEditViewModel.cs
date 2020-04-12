using System.Collections.Generic;

namespace WebApp.ViewModels.Film
{
    public class FilmEditViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<Genre> GenreViewModels { get; set; }
        
        public class Genre
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public bool Checked { get; set; }
            public Genre()
            {
                Checked = false;
            }
        }
    }
}