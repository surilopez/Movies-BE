using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Movies_BE.DTOs;

namespace Movies_BE.Controllers
{
    [Route("api/Acounts")]
    [ApiController]
    public class AcountsController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IConfiguration configuration;

        public AcountsController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration
            )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
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
                isPersistent:false, lockoutOnFailure:false);

            if (result.Succeeded)
            {
                return await BuildToken(credentials);
            }
            else {
                return BadRequest("Login Failed");
            }
        }


        //-------------------------OTHER FUNCTIONS-------------------------------------------------------------
        private async Task<AuthenticationResponse> BuildToken(CredentialsUser credentials)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email,credentials.email),
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
