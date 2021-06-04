using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MoviesApi.DTOs;
using MoviesApi.Entities;
using MoviesApi.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesApi.Repositories
{
    public class MovieRepository :MappingRepository<Movie>
    {
        private IFileStorageService fileStorageService;
        private string containerName = "movies";

        public MovieRepository(ApplicationDbContext applicationDbContext, IMapper mapper, IFileStorageService fileStorageService) : base (applicationDbContext, mapper)
        {
            this.fileStorageService = fileStorageService;
        }

        public async Task<MoviePostGetDTO> AddGet()
        {
            var movieTheaters = await applicationDb.MovieTheaters.OrderBy(mt => mt.Name).ToListAsync();
            var genres = await applicationDb.Genres.OrderBy(g => g.Name).ToListAsync();

            var movieTheatersDTO = _mapper.Map<List<MovieTheaterDTO>>(movieTheaters);
            var genresDTO = _mapper.Map<List<GenreDTO>>(genres);

            return new MoviePostGetDTO() { Genres = genresDTO, MovieTheaters = movieTheatersDTO };
        }

        public async Task<MovieDTO> GetDTOById(int id)
        {
            var movie = await applicationDb.Movies
                .Include(x => x.Genres)
                .Include(x => x.MovieTheaters)
                .Include(x => x.MoviesActors).ThenInclude(x => x.Actor)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (movie == null)
            {
                return null;
            }

            var dto = _mapper.Map<MovieDTO>(movie);
            dto.Actors = dto.Actors.OrderBy(x => x.Order).ToList();

            return dto;
        }
        public virtual async Task<Movie> AddDTO(MovieCreationDTO movieCreationDTO)
        {
            var movie = _mapper.Map<Movie>(movieCreationDTO);

            // another way without automapper (ignored in AutomapperProfile)
            if (movieCreationDTO.GenresIds.Count > 0)
            {
                var genres = applicationDb.Genres.ToList();
                foreach (var id in movieCreationDTO.GenresIds)
                {
                    movie.Genres.Add(genres.First(g => g.Id == id));
                }
            }

            if (movieCreationDTO.MovieTheatersIds.Count > 0)
            {
                var movieTheaters = applicationDb.MovieTheaters.ToList();
                foreach (var id in movieCreationDTO.MovieTheatersIds)
                {
                    movie.MovieTheaters.Add(movieTheaters.First(mt => mt.Id == id));
                }
            }

            if (movieCreationDTO.Poster != null)
            {
                movie.Poster = await fileStorageService.SaveFile(containerName, movieCreationDTO.Poster);
            }

            AnnotateActorsOrder(movie);

            return await Attach(movie);
        }

        private void AnnotateActorsOrder (Movie movie)
        {
            if (movie.MoviesActors != null)
            {
                for (int i = 0; i < movie.MoviesActors.Count; i++)
                {
                    movie.MoviesActors[i].Order = i;
                }
            }
        }

        public async Task<HomeDTO> GetHome()
        {
            var top = 6;
            var today = DateTime.Today;

            var upcomingReleases = await applicationDb.Movies
                .Where(x => x.ReleaseDate > today)
                .OrderBy(x => x.ReleaseDate)
                .Take(top)
                .ToListAsync();

            var inTheaters = await applicationDb.Movies
                .Where(x => x.InTheaters)
                .OrderBy(x => x.ReleaseDate)
                .Take(top)
                .ToListAsync();

            var homeDTO = new HomeDTO();
            homeDTO.UpcomingReleases = _mapper.Map<List<MovieDTO>>(upcomingReleases);
            homeDTO.InTheaters = _mapper.Map<List<MovieDTO>>(inTheaters);

            return homeDTO;
        }

        public async Task<MoviePutGetDTO> PutGet (int id)
        {
            var movie = await GetDTOById(id);
            if (movie == null) { return null;}

            var genresSelectedIds = movie.Genres.Select(x => x.Id).ToList();
            var nonSelectedGenres = await applicationDb.Genres.Where(x => !genresSelectedIds.Contains(x.Id))
                .ToListAsync();

            var movieTheatersIds = movie.MovieTheaters.Select(x => x.Id).ToList();
            var nonSelectedMovieTheaters = await applicationDb.MovieTheaters.Where(x => !movieTheatersIds.Contains(x.Id))
                .ToListAsync();

            var nonSelectedGenresDTOs = _mapper.Map<List<GenreDTO>>(nonSelectedGenres);
            var nonSelectedMovieTheatersDTOs = _mapper.Map<List<MovieTheaterDTO>>(nonSelectedMovieTheaters);

            var response = new MoviePutGetDTO();
            response.Movie = movie;
            response.SelectedGenres = movie.Genres;
            response.NonSelectedGenres = nonSelectedGenresDTOs;
            response.SelectedMovieTheaters = movie.MovieTheaters;
            response.NonSelectedMovieTheaters = nonSelectedMovieTheatersDTOs;
            response.Actors = movie.Actors;

            return response;
        }

        public async Task<Movie> Update(int id, MovieCreationDTO movieCreationDTO)
        {

            var movie = await applicationDb.Movies.Include(x => x.MoviesActors)
                .Include(x => x.Genres)
                .Include(x => x.MovieTheaters)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (movie == null) { return null; }

            movie = _mapper.Map(movieCreationDTO, movie);

            if (movieCreationDTO.GenresIds.Count > 0)
            {
                var genres = applicationDb.Genres.ToList();
                foreach (var genreId in movieCreationDTO.GenresIds)
                {
                    movie.Genres.Add(genres.First(g => g.Id == genreId));

                }
            }

            if (movieCreationDTO.MovieTheatersIds.Count > 0)
            {
                var movieTheaters = applicationDb.MovieTheaters.ToList();
                foreach (var movieTheaterId in movieCreationDTO.MovieTheatersIds)
                {
                    movie.MovieTheaters.Add(movieTheaters.First(mt => mt.Id == movieTheaterId));
                }
            }


            if (movieCreationDTO.Poster != null)
            {
                movie.Poster = await fileStorageService.EditFile(containerName, movieCreationDTO.Poster, movie.Poster);
            }

            AnnotateActorsOrder(movie);

            try
            {
                await applicationDb.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("{0} Exception caught.", ex);
            }

            return movie;

        }

        public async Task<IQueryable<Movie>> Filter (FilterMovieDTO filterMovieDTO)
        {
            var moviesQueryable = applicationDb.Movies.AsQueryable();
            if (!string.IsNullOrEmpty(filterMovieDTO.Name))
            {
                moviesQueryable = moviesQueryable.Where(x => x.Name.Contains(filterMovieDTO.Name));
            }

            if (filterMovieDTO.InTheaters)
            {
                moviesQueryable = moviesQueryable.Where(x => x.InTheaters);
            }

            if (filterMovieDTO.UpcomingReleases)
            {
                var today = DateTime.Today;
                moviesQueryable = moviesQueryable.Where(x => x.ReleaseDate > today);
            }
            //
            if(filterMovieDTO.GenreId != 0)
            {
                moviesQueryable = moviesQueryable
                    .Where(x => x.Genres.Select(y => y.Id)
                    .Contains(filterMovieDTO.GenreId));
            }

           
            return moviesQueryable;
        }

        public async Task<List<MovieDTO>> MoviesToDTO(IQueryable<Movie> moviesQueryable, FilterMovieDTO filterMovieDTO)
        {
            var movies = await moviesQueryable.OrderBy(x => x.Name).Paginate(filterMovieDTO.PaginationDTO)
                .ToListAsync();

            return _mapper.Map<List<MovieDTO>>(movies);
        }

        public async Task<Movie> DeleteMovie(int id)
        {
            var movie = await GetById(id);
            if (movie == null)
            {
                return null;
            }
            await Delete(movie);

            await fileStorageService.DeleteFile(movie.Poster, containerName);

            return movie;          
        }
    }
}
