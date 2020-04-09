using System;
using System.Linq;
using DataLayer;
using DataLayer.Entity;
using DataLayer.Repository;
using Microsoft.EntityFrameworkCore;
using Test.Fixtures;
using WebApp;
using Xunit;
using Xunit.Abstractions;

namespace Test.Entity
{
    [Collection("Database Collection")]
    public class FilmEntityTest : IDisposable
    {
        private readonly FilmRepo _filmRepo;
        private readonly FilmContext _context;
            
        public FilmEntityTest(DbFixture fixture)
        {
            _context = fixture.Context;
            _filmRepo = new FilmRepo(_context);
            
           DbTestData.SeedData(_context);
        }

        public void Dispose()
        {
            DbTestData.CleanDb(_context);
        }

        
        [Fact]
        public void GetMethodOnRepo()
        {
            Assert.True(_filmRepo.GetAll().Any(f=>f.Title=="James Bond"));
        }
    }
}