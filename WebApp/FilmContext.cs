using System;
using DataLayer.Entity;
using Microsoft.EntityFrameworkCore;

namespace WebApp
{
    public class FilmContext : DbContext
    {
        public FilmContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Film> Films { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Participant> Participants { get; set; }
        
        public DbSet<FilmActor> FilmActors { get; set; }
        public DbSet<FilmGenre> FilmGenres { get; set; }
        public DbSet<FilmDirector> FilmDirectors { get; set; }
        public DbSet<FilmProducer> FilmProducers { get; set; }
        public DbSet<FilmWriter> FilmWriters { get; set; }
        public DbSet<FilmIdeaAuthor> FilmIdeaAuthors { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FilmActor>()
                .HasKey(e => new {e.FilmId, e.ParticipantId});
            
            modelBuilder.Entity<FilmGenre>()
                .HasKey(e => new {e.FilmId, e.GenreId});
            
            modelBuilder.Entity<FilmDirector>()
                .HasKey(e => new {e.FilmId, e.ParticipantId});
            
            modelBuilder.Entity<FilmProducer>()
                .HasKey(e => new {e.FilmId, e.ParticipantId});
            
            modelBuilder.Entity<FilmWriter>()
                .HasKey(e => new {e.FilmId, e.ParticipantId});
            
            modelBuilder.Entity<FilmIdeaAuthor>()
                .HasKey(e => new {e.FilmId, e.ParticipantId});
        }
    }
}