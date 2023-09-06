using CodePulse.API.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;

        public AuthController(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            // Create IdentityUser Objekt
            var user = new IdentityUser
            {
                UserName = request.Email?.Trim(),
                Email = request.Email?.Trim()
            };

            //Create User
            var identityResualt = await userManager.CreateAsync(user, request.Password);

            if (identityResualt.Succeeded)
            {
                //Add Role To User
                identityResualt = await userManager.AddToRoleAsync(user, "Reader");

                if (identityResualt.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    if (identityResualt.Errors.Any())
                    {
                        foreach (var error in identityResualt.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
            }
            else
            {
                if (identityResualt.Errors.Any()) 
                {
                    foreach (var error in identityResualt.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return ValidationProblem(ModelState);
        }


    }
}
