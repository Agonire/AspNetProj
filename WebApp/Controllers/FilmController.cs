using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public FilmController(IFilmRepo filmRepo, IGenreRepo genreRepo, IMapper mapper)
        {
            _filmRepo = filmRepo;
            _genreRepo = genreRepo;
            _mapper = mapper;
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
                GenreSelections =
                    _mapper.Map<List<Genre>, List<FilmViewModel.GenreSelection>>(_genreRepo.GetAll().ToList())
            };

            return View(filmViewModel);
        }

        // NEEDSWORK
        // POST: Film/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(FilmViewModel filmViewModel)
        {
            if (!ModelState.IsValid) return View(filmViewModel);
            
            var film = _mapper.Map<Film>(filmViewModel);

            film.FilmGenres = new List<FilmGenre>();
            
            
            var checkedGenres = filmViewModel.GenreSelections.Where(gs => gs.Checked).ToList();
         

            // Add genre to film
            foreach (var genreSelection in filmViewModel.GenreSelections.Where(genreSelection =>
                genreSelection.Checked))
            {
                film.FilmGenres.Add(new FilmGenre()
                {
                    FilmId = filmViewModel.Id,
                    GenreId = genreSelection.Id
                });
            }

            _filmRepo.Create(film);
            _filmRepo.Save();

            return RedirectToAction(nameof(Index));
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

            var filmViewModel = _mapper.Map<FilmViewModel>(film);
            filmViewModel.GenreSelections = _mapper.Map<List<Genre>, List<FilmViewModel.GenreSelection>>(_genreRepo.GetAll().ToList());

            var filmGenres = film.FilmGenres.Select(filmGenre => filmGenre.Genre).ToList();

            // GenreSelection.check = true, if genre exists inside filmGenres
            foreach (var genreSelection in filmViewModel.GenreSelections
                .Where(genreSelection => filmGenres.Any(g => g.Id == genreSelection.Id)))
            {
                genreSelection.Checked = true;
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
            if (id != filmViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var newFilmData = _mapper.Map<Film>(filmViewModel);

                filmViewModel.GenreSelections.RemoveAll(gs => !gs.Checked);

                newFilmData.FilmGenres = _mapper.Map<FilmViewModel, List<FilmGenre>>(filmViewModel);
                
                try
                {
                    _filmRepo.Update(newFilmData);
                    _filmRepo.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilmExists(newFilmData.Id))
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