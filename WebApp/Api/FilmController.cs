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
        private readonly IFilmRepo _filmRepo;

        public FilmController(IFilmRepo filmRepo)
        {
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
            var film = _filmRepo.Find(f => f.Id == id).FirstOrDefault();

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
                _filmRepo.Update(film);

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
        public ActionResult<Film> PostFilm(Film film)
        {
            _filmRepo.Create(film);
            _filmRepo.Save();

            return CreatedAtAction("GetFilm", new {id = film.Id}, film);
        }

        // DELETE: api/FilmApi/5
        [HttpDelete("{id}")]
        public ActionResult<Film> DeleteFilm(int id)
        {
            var film = _filmRepo.Find(f=>f.Id == id).FirstOrDefault();
            if (film == null)
            {
                return NotFound();
            }

            _filmRepo.Delete(film);
            _filmRepo.Save();
            
            return film;
        }

        private bool FilmExists(int id)
        {
            return _filmRepo.Find(f => f.Id == id).Any();
        }
    }
}