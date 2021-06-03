using AutoMapper;
using MoviesApi.DTOs;
using MoviesApi.Entities;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesApi.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles(GeometryFactory geometryFactory)
        {
            CreateMap<GenreDTO, Genre>().ReverseMap();
            CreateMap<GenreCreationDTO, Genre>().ReverseMap();

            CreateMap<ActorCreationDTO, Actor>().ReverseMap()
                .ForMember(x => x.Picture, options => options.Ignore());

            CreateMap<MovieTheater, MovieTheaterDTO>()
                .ForMember(x => x.Latitude, dto => dto.MapFrom(prop => prop.Location.Y))
                .ForMember(x => x.Longitude, dto => dto.MapFrom(prop => prop.Location.X));

            CreateMap<MovieTheaterCreationDTO, MovieTheater>()
                .ForMember(x => x.Location, x => x.MapFrom(dto =>
                geometryFactory.CreatePoint(new Coordinate(dto.Longitude, dto.Latitude))));

            CreateMap<MovieCreationDTO, Movie>()
                .ForMember(x => x.Poster, options => options.Ignore())
                .ForMember(x => x.Genres, options => options.Ignore())
                .ForMember(x => x.MovieTheaters, options => options.Ignore())
                //.ForMember(x => x.Genres, options => options.MapFrom(MapMovieGenres))
                //.ForMember(x => x.MovieTheaters, options => options.MapFrom(MapMovieTheatersMovies))
                .ForMember(x => x.MoviesActors, options => options.MapFrom(MapMoviesActors));

            CreateMap<Movie, MovieDTO>()
                .ForMember(x => x.Genres, options => options.MapFrom(MapMoviesGenres))
                .ForMember(x => x.MovieTheaters, options => options.MapFrom(MapMovieMovietheater))
                .ForMember(x => x.Actors, options => options.MapFrom(MapMovieActor));

            CreateMap<Actor, ActorsMovieDTO>().ReverseMap();
        }

        private List<MovieTheaterDTO> MapMovieMovietheater(Movie movie, MovieDTO movieDTO)
        {
            var result = new List<MovieTheaterDTO>();

            if (movie.MovieTheaters.Count > 0)
            {
               foreach ( var movieTheater in movie.MovieTheaters)
                {
                    result.Add(new MovieTheaterDTO()
                    {
                        Id = movieTheater.Id,
                        Name = movieTheater.Name,
                        Latitude = movieTheater.Location.Y,
                        Longitude = movieTheater.Location.X
                    });

                }
               
            }

            return result;
        }
        private List<GenreDTO> MapMoviesGenres(Movie movie, MovieDTO movieDTO)
        {
            var result = new List<GenreDTO>();

            if(movie.Genres.Count > 0)
            {
                foreach (var genre in movie.Genres)
                {
                    result.Add(new GenreDTO() { Id = genre.Id, Name = genre.Name });
                }
            }

            return result;
        }

        private List<ActorsMovieDTO> MapMovieActor( Movie movie , MovieDTO movieDTO)
        {
            var result = new List<ActorsMovieDTO>();
           
            if (movie.MoviesActors.Count > 0)
            {
                foreach (var actor in movie.MoviesActors)
                {
                    result.Add(new ActorsMovieDTO()
                    {
                        Id = actor.ActorId,
                        Name = actor.Actor.Name,
                        Character = actor.Character,
                        Picture = actor.Actor.Picture,
                        Order = actor.Order
                    });
                }
            }

            return result;
        }

        private List<MoviesActors> MapMoviesActors(MovieCreationDTO movieCreationDTO, Movie movie)
        {
            var result = new List<MoviesActors>();
            if (movieCreationDTO.Actors == null) { return result; }

            foreach (var actor in movieCreationDTO.Actors)
            {
                result.Add(new MoviesActors() { ActorId = actor.Id, Character = actor.Character });
            }

            return result;
        }

        private List<Genre> MapMovieGenres(MovieCreationDTO movieCreationDTO, Movie movie)
        {
            var result = new List<Genre>();
            if (movieCreationDTO.GenresIds == null) { return result; }

            foreach (var id in movieCreationDTO.GenresIds)
            {
                result.Add(new Genre() { Id = id });
            }

            return result;
        }

        private List<MovieTheater> MapMovieTheatersMovies(MovieCreationDTO movieCreationDTO, Movie movie)
        {
            var result = new List<MovieTheater>();
            if (movieCreationDTO.MovieTheatersIds == null) { return result; }

            foreach (var id in movieCreationDTO.MovieTheatersIds)
            {
                result.Add(new MovieTheater() { Id = id });
            }

            return result;
        }

    }
}

