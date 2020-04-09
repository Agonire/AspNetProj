using System;
using System.Linq;
using DataLayer;
using DataLayer.Entity;
using Microsoft.EntityFrameworkCore;
using Test.Fixtures;
using WebApp;
using Xunit;

namespace Test.Entity
{
    [Collection("Database Collection")]
    public class GenreEntityTest: IDisposable
    {
        private readonly FilmContext _context;
            
        public GenreEntityTest(DbFixture fixture)
        {
            _context = fixture.Context;
            
            DbTestData.SeedData(_context);
        }

        public void Dispose()
        {
            DbTestData.CleanDb(_context);
        }
        
        
        [Fact]
        public void DeletingGenreCascades()
        {
            var initCount = _context.FilmGenres.Count();

            var filmGenre = _context.FilmGenres
                .Include(fg=>fg.Genre)
                .FirstOrDefault();
            
            if (filmGenre != null) _context.Genres.Remove(filmGenre.Genre);
            _context.SaveChanges();

            var newCount = _context.FilmGenres.Count();

            Assert.True(initCount > newCount);
        }
    }
}