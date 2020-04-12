using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataLayer;
using DataLayer.Contract;
using DataLayer.Entity;
using DataLayer.Repository;

namespace WebApp.Controllers
{
    public class GenreController : Controller
    {
        private readonly IGenreRepo _genreRepo;

        public GenreController(IGenreRepo genreRepo)
        {
            _genreRepo = genreRepo;
        }

        // GET: Genre
        public IActionResult Index()
        {
            return View(_genreRepo.GetAll().ToList());
        }

        // GET: Genre/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null) return NotFound();

            var genre = _genreRepo.Find(g => g.Id == id).FirstOrDefault();
            if (genre == null) return NotFound();

            return View(genre);
        }

        // GET: Genre/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Genre/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Genre genre)
        {
            if (ModelState.IsValid)
            {
                _genreRepo.Create(genre);
                _genreRepo.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(genre);
        }

        // GET: Genre/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();

            var genre = _genreRepo.Find(g=>g.Id == id).FirstOrDefault();
            if (genre == null) return NotFound();
            return View(genre);
        }

        // POST: Genre/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Genre genre)
        {
            if (id != genre.Id) return NotFound();

            if (!ModelState.IsValid) return View(genre);
            
            try
            {
                _genreRepo.Update(genre);
                _genreRepo.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GenreExists(genre.Id))
                    return NotFound();
                else
                    throw;
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Genre/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();

            var genre = _genreRepo.Find(g => g.Id == id).FirstOrDefault();
            if (genre == null) return NotFound();

            return View(genre);
        }

        // POST: Genre/Delete/5
        [HttpPost][ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var genre = _genreRepo.Find(g => g.Id == id).FirstOrDefault();
            // Null check ???
            _genreRepo.Delete(genre);
            _genreRepo.Save();
            return RedirectToAction(nameof(Index));
        }

        private bool GenreExists(int id)
        {
            return _genreRepo.GetAll().Any(e => e.Id == id);
        }
    }
}
