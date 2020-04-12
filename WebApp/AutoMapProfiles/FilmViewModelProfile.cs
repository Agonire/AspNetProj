using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DataLayer.Entity;
using WebApp.ViewModels;

namespace WebApp.AutoMapProfiles
{
    public class FilmViewModelProfile : Profile
    {
        public FilmViewModelProfile()
        {
            CreateMap<FilmViewModel, Film>()
                .ReverseMap();
            
            CreateMap<FilmViewModel, List<FilmGenre>>()
                .ConvertUsing<CustomMapping>();
            
            CreateMap<FilmViewModel.GenreSelection, FilmGenre>()
                .ForMember(dest => dest.GenreId, opt => opt
                    .MapFrom(src => src.Id))
                .ReverseMap();
        }
    }

    public class CustomMapping : ITypeConverter<FilmViewModel, List<FilmGenre>>
    {
        public List<FilmGenre> Convert(FilmViewModel source, List<FilmGenre> destination, ResolutionContext context)
        {
            var res = context.Mapper.Map<List<FilmViewModel.GenreSelection>, List<FilmGenre>>(source.GenreSelections);
            res.ForEach(fg=> fg.FilmId = source.Id);

            return res;
        }
    }
}
