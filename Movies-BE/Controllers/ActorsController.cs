using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies_BE.DTOs;
using Movies_BE.Entities;
using Movies_BE.Utilities;

namespace Movies_BE.Controllers
{
    [Route("api/actors")]
    [ApiController]
    public class ActorsController : ControllerBase
    {
        private readonly ApplicationDBContext context;
        private readonly IMapper mapper;
        private readonly IStorageFile storageFile;
        private readonly string container = "actors";

        public ActorsController(
            ApplicationDBContext context, 
            IMapper mapper,
            IStorageFile storageFile)
        {
            this.context = context;
            this.mapper = mapper;
            this.storageFile = storageFile;
        }
        [HttpGet]
        public async Task<ActionResult<List<ActorDTO>>> Get([FromQuery] PaginationDTO paginationDTO) {
            var queryable = context.Actors.AsQueryable();
            await HttpContext.InsertPaginationParamsOnHeader(queryable);
            var actors = await queryable.OrderBy(x => x.Name).Pagin(paginationDTO).ToListAsync();

            return mapper.Map<List<ActorDTO>>(actors);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] ActorAddDTO actorAddDTO)
        {
            var actor = mapper.Map<Actor>(actorAddDTO);

            if (actorAddDTO.Photo != null)
            {
               actor.Photo= await storageFile.SaveFiles(container, actorAddDTO.Photo);
            }

            context.Add(actor);
            await context.SaveChangesAsync();
            return NoContent();

        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await context.Actors.AnyAsync(x => x.id == id);
            if (!exist)
            {
                return NotFound();
            }
            context.Remove(new Actor() { id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
