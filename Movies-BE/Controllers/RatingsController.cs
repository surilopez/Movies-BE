using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies_BE.DTOs;
using Movies_BE.Entities;

namespace Movies_BE.Controllers
{
    [Route("api/Rating")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ApplicationDBContext context;

        public RatingsController(UserManager<IdentityUser> userManager,
            ApplicationDBContext applicationDBContext)
        {
            this.userManager = userManager;
            this.context = applicationDBContext;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post([FromBody] RatingDTO ratingDTO)
        {
            var email = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "email").Value;
            var user = await userManager.FindByEmailAsync(email);
            var userId = user.Id;

            var currentRating = await context.Ratings
                .FirstOrDefaultAsync(x => x.movieId == ratingDTO.movieId
                && x.userId == userId);

            if (currentRating == null)
            {
                var rating = new Rating();
                rating.movieId = ratingDTO.movieId;
                rating.score = ratingDTO.score;
                rating.userId = userId;
                context.Add(rating);
            }
            else
            {
                currentRating.score = ratingDTO.score;

            }
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
