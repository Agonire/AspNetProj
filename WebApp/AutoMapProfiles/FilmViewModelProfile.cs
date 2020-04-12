using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DataLayer.Entity;
using WebApp.ViewModels;
using WebApp.ViewModels.Film;

namespace WebApp.AutoMapProfiles
{
    public class FilmViewModelProfile : Profile
    {
        public FilmViewModelProfile()
        {
            CreateMap<FilmIndexViewModel, Film>()
                .ReverseMap();
            
            CreateMap<FilmDetailsViewModel, Film>()
                .ReverseMap();
            
            CreateMap<FilmEditViewModel, Film>()
                .ReverseMap();
            
            CreateMap<FilmEditViewModel, List<FilmGenre>>()
                .ConvertUsing<CustomMapping>();

            CreateMap<FilmEditViewModel.Genre, Genre>()
                .ReverseMap();
            
            CreateMap<FilmEditViewModel.Genre, FilmGenre>()
                .ForMember(dest => dest.GenreId, opt => opt
                    .MapFrom(src => src.Id))
                .ReverseMap();
        }
    }

    public class CustomMapping : ITypeConverter<FilmEditViewModel, List<FilmGenre>>
    {
        public List<FilmGenre> Convert(FilmEditViewModel source, List<FilmGenre> destination, ResolutionContext context)
        {
            var result = context.Mapper.Map<List<FilmEditViewModel.Genre>, List<FilmGenre>>(source.GenreViewModels);
            result.ForEach(fg=> fg.FilmId = source.Id);

            return result;
        }
    }
}
