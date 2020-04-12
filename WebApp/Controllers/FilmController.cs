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
using Microsoft.EntityFrameworkCore.Internal;
using WebApp.ViewModels;
using WebApp.ViewModels.Film;

namespace WebApp.Controllers
{
    public class FilmController : Controller
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
            var result = 
                _mapper.Map<IEnumerable<Film>, IEnumerable<FilmIndexViewModel>>(_filmRepo.GetAll());
            
            return View(result);
        }

        // GET: Film/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null) return NotFound();

            var film = _filmRepo.Find(f => f.Id == id).FirstOrDefault();
            
            if (film == null) return NotFound();

            // Map film to filmDetailsViewModel
            var filmDetailsViewModel = _mapper.Map<FilmDetailsViewModel>(film);

            return View(filmDetailsViewModel);
        }

        // GET: Film/Create
        public IActionResult Create()
        {
            var filmViewModel = new FilmEditViewModel
            {
                GenreViewModels =
                    _mapper.Map<List<Genre>, List<FilmEditViewModel.Genre>>(_genreRepo.GetAll().ToList())
            };

            return View(filmViewModel);
        }

        // POST: Film/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(FilmEditViewModel filmEditViewModel)
        {
            if (!ModelState.IsValid) return View(filmEditViewModel);
            
            var film = _mapper.Map<Film>(filmEditViewModel);

            filmEditViewModel.GenreViewModels.RemoveAll(gs => !gs.Checked);            

            film.FilmGenres = _mapper.Map<FilmEditViewModel, List<FilmGenre>>(filmEditViewModel);            

            _filmRepo.Create(film);
            _filmRepo.Save();

            return RedirectToAction(nameof(Index));
        }

        // GET: Film/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();

            var film = _filmRepo.Find(f => f.Id == id).FirstOrDefault();

            if (film == null) return NotFound();
            

            var filmViewModel = _mapper.Map<FilmEditViewModel>(film);
            filmViewModel.GenreViewModels = _mapper.Map<List<Genre>, List<FilmEditViewModel.Genre>>(_genreRepo.GetAll().ToList());

            var filmGenres = film.FilmGenres.Select(filmGenre => filmGenre.Genre).ToList();

            filmViewModel.GenreViewModels.Where(gvm => filmGenres.Any(g => g.Id == gvm.Id)).ToList()
                .ForEach(gvm=> gvm.Checked = true);

            return View(filmViewModel);
        }

        // POST: Film/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, FilmEditViewModel filmEditViewModel)
        {
            if (id != filmEditViewModel.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var newFilmData = _mapper.Map<Film>(filmEditViewModel);

                filmEditViewModel.GenreViewModels.RemoveAll(gs => !gs.Checked);

                newFilmData.FilmGenres = _mapper.Map<FilmEditViewModel, List<FilmGenre>>(filmEditViewModel);
                
                try
                {
                    _filmRepo.Update(newFilmData);
                    _filmRepo.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilmExists(newFilmData.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(filmEditViewModel);
        }

        // GET: Film/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();

            var film = _filmRepo.Find(f => f.Id == id).FirstOrDefault();
            
            if (film == null) return NotFound();

            var result = _mapper.Map<FilmIndexViewModel>(film);

            return View(result);
        }

        // POST: Film/Delete/5
        [HttpPost][ActionName("Delete")]
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