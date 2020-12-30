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
    [ApiController]
    [Route("api/Theaters")]
    public class TheaterController : ControllerBase
    {
        private readonly ApplicationDBContext context;
        private readonly IMapper mapper;

        public TheaterController(
            ApplicationDBContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }


        [HttpPost]
        public async Task<ActionResult> Post([FromBody] TheaterAddDTO theaterAddDTO)
        {
            var theather = mapper.Map<Theaters>(theaterAddDTO);
            context.Add(theather);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
