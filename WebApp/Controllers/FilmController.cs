using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataLayer;
using DataLayer.Contract;
using DataLayer.Entity;
using DataLayer.Repository;
using Microsoft.AspNetCore.Http;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class FilmController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly IFilmRepo _filmRepo;
        private readonly IGenreRepo _genreRepo;

        public FilmController(IFilmRepo filmRepo, IGenreRepo genreRepo)
        {
            _filmRepo = filmRepo;
            _genreRepo = genreRepo;
        }

        // GET: Film
        public IActionResult Index()
        {
            return View(_filmRepo.GetAll());
        }

        // GET: Film/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = _filmRepo.Find(f => f.Id == id).FirstOrDefault();
            if (film == null)
            {
                return NotFound();
            }

            return View(film);
        }

        // GET: Film/Create
        public IActionResult Create()
        {
            var filmViewModel = new FilmViewModel
            {
                Film = new Film(),
                GenreSelections = new List<FilmViewModel.GenreSelection>()
            };

            foreach (var genre in _genreRepo.GetAll())
            {
                filmViewModel.GenreSelections.Add(new FilmViewModel.GenreSelection()
                {
                    Genre = genre
                });
            }

            return View(filmViewModel);
        }

        // POST: Film/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(FilmViewModel filmViewModel)
        {
            if (ModelState.IsValid)
            {
                var film = filmViewModel.Film;
                film.FilmGenres = new List<FilmGenre>();
                _filmRepo.Create(film);

                // Add genre to film
                foreach (var genreSelection in filmViewModel.GenreSelections.Where(genreSelection => genreSelection.Checked))
                {
                    film.FilmGenres.Add(new FilmGenre()
                    {
                        Film = film,
                        GenreId = genreSelection.Genre.Id
                    });
                }

                _filmRepo.Save();

                return RedirectToAction(nameof(Index));
            }

            return View(filmViewModel);
        }

        // GET: Film/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = _filmRepo.Find(f => f.Id == id).FirstOrDefault();

            if (film == null)
            {
                return NotFound();
            }

            var filmViewModel = new FilmViewModel
            {
                Film = film,
                GenreSelections = new List<FilmViewModel.GenreSelection>()
            };

            var genres = film.FilmGenres.Select(filmGenre => filmGenre.Genre).ToList();

            foreach (var genre in _genreRepo.GetAll())
            {
                var genreSelector = new FilmViewModel.GenreSelection()
                {
                    Genre = genre
                };
                
                if (genres.Any(g=>g.Id == genre.Id))
                    genreSelector.Checked = true;

                filmViewModel.GenreSelections.Add(genreSelector);
            }
            return View(filmViewModel);
        }

        // POST: Film/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, FilmViewModel filmViewModel)
        {
            if (id != filmViewModel.Film.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var filmToUpdate = _filmRepo.Find(f => f.Id == id).FirstOrDefault();
                if (filmToUpdate == null) return RedirectToAction(nameof(Index));

                var newFilmData = new Film {Title = filmViewModel.Film.Title, FilmGenres = new List<FilmGenre>()};
                
                
                foreach (var genreSelection in filmViewModel.GenreSelections
                    .Where(s => s.Checked))
                {
                    newFilmData.FilmGenres.Add(new FilmGenre()
                    {
                        FilmId = filmToUpdate.Id,
                        GenreId = genreSelection.Genre.Id
                    });
                }

                try
                {
                    _filmRepo.CascadeUpdate(filmToUpdate, newFilmData);
                    _filmRepo.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilmExists(filmViewModel.Film.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(filmViewModel);
        }

        // GET: Film/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = _filmRepo.Find(f => f.Id == id).FirstOrDefault();
            if (film == null)
            {
                return NotFound();
            }

            return View(film);
        }

        // POST: Film/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var film = _filmRepo.Find(f => f.Id == id).FirstOrDefault();
            _filmRepo.Delete(film);
            _filmRepo.Save();
            return RedirectToAction(nameof(Index));
        }

        private bool FilmExists(int id)
        {
            return _filmRepo.GetAll().Any(e => e.Id == id);
        }
    }
}