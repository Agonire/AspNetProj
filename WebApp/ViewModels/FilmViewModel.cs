using System.Collections.Generic;
using System.Linq;
using DataLayer.Entity;
using DataLayer.Repository;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels
{
    public class FilmViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<GenreSelection> GenreSelections { get; set; }

        public class GenreSelection
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public bool Checked { get; set; }
            public GenreSelection()
            {
                Checked = false;
            }
        }
    }
}