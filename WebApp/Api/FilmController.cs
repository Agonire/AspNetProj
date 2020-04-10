using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataLayer;
using DataLayer.Contract;
using DataLayer.Entity;
using DataLayer.Repository;

namespace WebApp.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmController : ControllerBase
    {
        private readonly FilmContext _context;
        private readonly IFilmRepo _filmRepo;

        public FilmController(FilmContext context, IFilmRepo filmRepo)
        {
            _context = context;
            _filmRepo = filmRepo;
        }

        // GET: api/FilmApi
        [HttpGet]
        public ActionResult<IEnumerable<Film>> GetFilms()
        {
            return _filmRepo.GetAll().ToList();
        }

        // GET: api/FilmApi/5
        [HttpGet("{id}")]
        public ActionResult<Film> GetFilm(int id)
        {
            var film = _filmRepo.Find(f=>f.Id == id).FirstOrDefault();

            if (film == null)
            {
                return NotFound();
            }

            return film;
        }

        // PUT: api/FilmApi/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public IActionResult PutFilm(int id, Film film)
        {
            if (id != film.Id)
            {
                return BadRequest();
            }

            try
            {
                var oldFilm = _filmRepo.Find(f => f.Id == id).FirstOrDefault();
                if (oldFilm != null)
                {
                    if(film.FilmGenres == null)
                        film.FilmGenres = new List<FilmGenre>();
                    _filmRepo.CascadeUpdate(oldFilm, film);
                }

                _filmRepo.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FilmExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/FilmApi
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Film>> PostFilm(Film film)
        {
            _context.Films.Add(film);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFilm", new { id = film.Id }, film);
        }

        // DELETE: api/FilmApi/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Film>> DeleteFilm(int id)
        {
            var film = await _context.Films.FindAsync(id);
            if (film == null)
            {
                return NotFound();
            }

            _context.Films.Remove(film);
            await _context.SaveChangesAsync();

            return film;
        }

        private bool FilmExists(int id)
        {
            return _context.Films.Any(e => e.Id == id);
        }
    }
}
