using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Movies_BE.DTOs;
using Movies_BE.Utilities;

namespace Movies_BE.Controllers
{
    [Route("api/Acounts")]
    [ApiController]
    public class AcountsController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IConfiguration configuration;
        private readonly ApplicationDBContext context;
        private readonly IMapper mapper;

        public AcountsController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration,
            ApplicationDBContext context,
            IMapper mapper
            )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.context = context;
            this.mapper = mapper;
        }

        //-----------------------------Endpoints POST-----------------------------------------------------------------------------------------
        [HttpPost("Add")]
        public async Task<ActionResult<AuthenticationResponse>> Add([FromBody] CredentialsUser credentials)
        {
            var user = new IdentityUser { UserName = credentials.email, Email = credentials.email };
            var result = await userManager.CreateAsync(user, credentials.password);

            if (result.Succeeded)
            {
                return await BuildToken(credentials);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }



        [HttpPost("Login")]
        public async Task<ActionResult<AuthenticationResponse>> Login([FromBody] CredentialsUser credentials)
        {
            var result = await signInManager.PasswordSignInAsync(credentials.email, credentials.password,
                isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return await BuildToken(credentials);
            }
            else
            {
                return BadRequest("Login Failed");
            }
        }

        [HttpPost("AddRoleAdmin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy ="isAdmin")]
        public async Task<ActionResult> AddRoleAdminn([FromBody] string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            await userManager.AddClaimAsync(user, new Claim("role", "admin"));
            return NoContent();
        }

        [HttpPost("RemoveRoleAdmin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "isAdmin")]
        public async Task<ActionResult> RemoveRoleAdmin([FromBody] string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            await userManager.RemoveClaimAsync(user, new Claim("role", "admin"));
            return NoContent();
        }

        //-----------------------------Endpoints GET-----------------------------------------------------------

        [HttpGet("UserList")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "isAdmin")]
        public async Task<ActionResult<List<UserDTO>>> UserList([FromQuery] PaginationDTO paginationDTO)
        {
            var queriable = context.Users.AsQueryable();
            await HttpContext.InsertPaginationParamsOnHeader(queriable);
            var users = await queriable.OrderBy(x => x.Email).Pagin(paginationDTO).ToListAsync();
            return mapper.Map<List<UserDTO>>(users);
        }

        //-------------------------OTHER FUNCTIONS-------------------------------------------------------------
        private async Task<AuthenticationResponse> BuildToken(CredentialsUser credentials)
        {
            var claims = new List<Claim>()
            {
                new Claim("email",credentials.email)
            };

            var user = await userManager.FindByEmailAsync(credentials.email);
            var claimsDB = await userManager.GetClaimsAsync(user);
            claims.AddRange(claimsDB);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwtKey"]));

            var crededntial = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddDays(1);

            var token = new JwtSecurityToken(issuer: null, audience: null, claims: claims,
                expires: expiration, signingCredentials: crededntial);

            return new AuthenticationResponse()
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = expiration
            };
        }
    }
}
