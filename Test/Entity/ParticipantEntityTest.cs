using System;
using System.Data.Common;
using System.Linq;
using DataLayer;
using DataLayer.Entity;
using Microsoft.EntityFrameworkCore;
using Test.Fixtures;
using WebApp;
using Xunit;
using FluentAssertions;
using MySql.Data.MySqlClient;

namespace Test.Entity
{
    [Collection("Database Collection")]
    public class ParticipantEntityTest : IDisposable
    {
        private readonly DbContextOptions<FilmContext> Options;

        public ParticipantEntityTest(DbFixture fixture)
        {
            Options = fixture.Options;
            using var context = new FilmContext(Options);
            DbTestData.SeedData(context);
        }

        public void Dispose()
        {
            using var context = new FilmContext(Options);
            DbTestData.CleanDb(context);
        }

        [Fact]
        public void AddingBadDataThrowsException()
        {
            var participant = new Participant() {FirstName = "Doesnt have a last name", LastName = "uiou"};

            using (var context = new FilmContext(Options))
            {
                try
                {
                    context.Participants.Add(participant);
                    context.SaveChanges();
                }
                catch
                {
                    // ignored
                }
            }

            using (var context = new FilmContext(Options))
            {
                Assert.True(context.Participants.Any(p=>p.FirstName == "Doesnt have a last name"));
            }
        }
    }
}