using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace QuizBackend.Controllers
{
    public class credentials
    {
        public String Email { get; set; }
        public String Password { get; set; }
    }


    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountController : Controller
    {
        readonly UserManager<IdentityUser> userManager;
        readonly SignInManager<IdentityUser> SignInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.SignInManager = signInManager;
        }

        [HttpPost]
        public async Task<ActionResult> Register([FromBody] credentials credentials)
        {
            var user = new IdentityUser
            {
                UserName = credentials.Email,
                Email = credentials.Email
            };
            var result = await userManager.CreateAsync(user, credentials.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await SignInManager.SignInAsync(user, isPersistent: false);

            //var claims = new Claim[] {
            //    new Claim(JwtRegisteredClaimNames.Sub,user.Id)
            //};

            //var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is the secret phrase"));
            //var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            //var jwt = new JwtSecurityToken(signingCredentials: signingCredentials, claims: claims);
            //return Ok(new JwtSecurityTokenHandler().WriteToken(jwt));

            return Ok(CreateToken(user));
        }


        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] credentials credentials)
        {
            var result = await SignInManager.PasswordSignInAsync(credentials.Email, credentials.Password, false, false);

            if(!result.Succeeded)
                return BadRequest();

            var user = await userManager.FindByEmailAsync(credentials.Email);

            //var claims = new Claim[] {
            //    new Claim(JwtRegisteredClaimNames.Sub,user.Id)
            //};

            //var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is the secret phrase"));
            //var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            //var jwt = new JwtSecurityToken(signingCredentials: signingCredentials, claims: claims);
            //return Ok(new JwtSecurityTokenHandler().WriteToken(jwt));

            return Ok(CreateToken(user));
        }

        string CreateToken(IdentityUser user)
        {
            var claims = new Claim[] {
                new Claim(JwtRegisteredClaimNames.Sub,user.Id)
            };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is the secret phrase"));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(signingCredentials: signingCredentials, claims: claims);
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }

}