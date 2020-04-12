using System.Collections.Generic;
using System.ComponentModel;
using AutoMapper;
using DataLayer.Entity;
using FluentAssertions;
using WebApp.AutoMapProfiles;
using WebApp.ViewModels.Film;
using Xunit;

namespace Test.Profile
{
    public class TestFilmGenreProfile
    {
        private IMapper _mapper;

        public TestFilmGenreProfile()
        {
            var configuration = new MapperConfiguration(cfg => { cfg.AddProfile<FilmViewModelProfile>(); });
            _mapper = configuration.CreateMapper();
        }

        [Theory]
        [InlineData(3, 2, 3)]
        [InlineData(14, 1, 2)]
        [InlineData(32, 4, 8)]
        public void SuccessMapFilmEditToFilmGenre(int filmId, int firstGenreId, int secondGenreId)
        {
            var source = new FilmEditViewModel()
            {
                Id = filmId,
                Title = "TestFilm",
                GenreViewModels = new List<FilmEditViewModel.Genre>()
                {
                    new FilmEditViewModel.Genre()
                    {
                        Id = firstGenreId,
                        Name = "TestGenre"
                    },
                    new FilmEditViewModel.Genre()
                    {
                        Id = secondGenreId,
                        Name = "TestGenre2"
                    }
                }
            };
            var result = _mapper.Map<FilmEditViewModel, List<FilmGenre>>(source);

            result.Should().Contain(fg => fg.FilmId == filmId && fg.GenreId == firstGenreId);
            result.Should().Contain(fg => fg.FilmId == filmId && fg.GenreId == secondGenreId);
        }

        [Theory]
        [InlineData(3, 2, 3)]
        [InlineData(14, 1, 2)]
        [InlineData(32, 4, 8)]
        public void SuccessMapFilmDetailsToFilm(int filmId, int firstGenreId, int secondGenreId)
        {
            var fevm = new FilmDetailsViewModel()
            {
                // Id = filmId,
                FilmGenres = new List<FilmGenre>()
                {
                    new FilmGenre()
                    {
                        FilmId = filmId,
                        GenreId = firstGenreId
                    },
                    new FilmGenre()
                    {
                        FilmId = filmId,
                        GenreId = secondGenreId
                    }
                }
            };

            var film = _mapper.Map<Film>(fevm);


            // film.Id.Should().Be(filmId);
            film.FilmGenres.Should().NotBeNull();
            film.FilmGenres.Should().Contain(fg => fg.FilmId == filmId && fg.GenreId == firstGenreId);
            film.FilmGenres.Should().Contain(fg => fg.FilmId == filmId && fg.GenreId == secondGenreId);
        }
    }
}