using System.Collections.Generic;
using System.ComponentModel;
using AutoMapper;
using DataLayer.Entity;
using WebApp.AutoMapProfiles;
using WebApp.ViewModels;
using Xunit;

namespace Test
{    
    [DisplayName("FilmGenreProfile")]
    public class TestFilmGenreProfile
    {
        [Fact]
        public void Test1()
        {
            var configuration = new MapperConfiguration(cfg => { cfg.AddProfile<FilmViewModelProfile>(); });

            var mapper = configuration.CreateMapper();

            var filmId = 3;
            var g1Id = 1;
            var g2Id = 2;

            var source = new FilmViewModel()
            {
                Id = filmId,
                Title = "TestFilm",
                GenreSelections = new List<FilmViewModel.GenreSelection>()
                {
                    new FilmViewModel.GenreSelection()
                    {
                        Id = g1Id,
                        Name = "TestGenre"
                    },
                    new FilmViewModel.GenreSelection()
                    {
                        Id = g2Id,
                        Name = "TestGenre2"
                    }
                }
            };
            //
            //
            var result = mapper.Map<FilmViewModel, List<FilmGenre>>(source);


            var filmGenres = new List<FilmGenre>()
            {
                new FilmGenre()
                {
                    FilmId = 1,
                    GenreId = 2
                }
            };

            // var fvm = mapper.Map<List<FilmGenre>, FilmViewModel>(filmGenres);

            Assert.True(result[0].FilmId == filmId && result[1].FilmId == filmId);
            Assert.True(result[0].GenreId == g1Id && result[1].GenreId == g2Id);
            //
            //

            // public class CustomResolver : AutoMapper.IValueResolver<FilmViewModel, List<FilmGenre>, List<FilmGenre>>
            // {
            //     public List<FilmGenre> Resolve(FilmViewModel source, List<FilmGenre> destination, List<FilmGenre> destMember, ResolutionContext resolutionContext)
            //     {
            //         throw new System.NotImplementedException();
            //     }
        }
    }
}