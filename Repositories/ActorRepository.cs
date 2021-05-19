using AutoMapper;
using MoviesApi.DTOs;
using MoviesApi.Entities;
using MoviesApi.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesApi.Repositories
{
    public class ActorRepository : MappingRepository<Actor>
    {
        private IFileStorageService fileStorageService;
        private string containerName = "actors";
        public ActorRepository(ApplicationDbContext context, IMapper mapper,IFileStorageService fileStorageService) : base (context, mapper)
        {
            this.fileStorageService = fileStorageService;
        }

        public virtual async Task<Actor> AddDTO(ActorCreationDTO actorCreationDTO)
        {
            var actor = _mapper.Map<Actor>(actorCreationDTO);

            if (actorCreationDTO.Picture != null)
            {
                actor.Picture = await fileStorageService.SaveFile(containerName, actorCreationDTO.Picture);
            }
            return await Add(actor);
        }

        public async Task<Actor> EditDTO(int id, ActorCreationDTO actorCreationDTO)
        {
            var editedActor = await GetById(id);
            var picture = editedActor.Picture;

            editedActor = _mapper.Map(actorCreationDTO, editedActor);

            if (editedActor.Picture != null)
            {
                editedActor.Picture = await fileStorageService.EditFile(containerName, actorCreationDTO.Picture, editedActor.Picture);
            }
            else
            {
                editedActor.Picture = picture;
            }

            await applicationDb.SaveChangesAsync();

            return editedActor;
        }

        public async Task<Actor>DeleteActor(int id)
        {
            var actor = await Delete(id);

            await fileStorageService.DeleteFile(actor.Picture, containerName);

            return actor;
        }
    }
}
