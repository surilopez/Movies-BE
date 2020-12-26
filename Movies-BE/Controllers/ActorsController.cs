using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Movies_BE.DTOs;
using Movies_BE.Entities;

namespace Movies_BE.Controllers
{
    [Route("api/actors")]
    [ApiController]
    public class ActorsController : ControllerBase
    {
        private readonly ApplicationDBContext context;
        private readonly IMapper mapper;

        public ActorsController(ApplicationDBContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        [HttpPost]
        public async Task<ActionResult> Post([FromForm] ActorAddDTO actorAddDTO)
        {
            //var actor = mapper.Map<Actor>(actorAddDTO);
            //context.Add(actor);
            //await context.SaveChangesAsync();
            return NoContent();

        }
    }
}
