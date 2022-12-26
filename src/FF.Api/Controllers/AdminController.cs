using FF.Api.Data.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FF.Core.Models;
using MediatR;
using FF.Api.Base;

namespace FF.Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AdminController : FlipsApiControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;

        public AdminController(UserManager<ApplicationUser> userManager, IMediator mediator) : base(mediator)
        {
            this.userManager = userManager;
        }

        [Authorize("ApiAndWebPolicy", Roles = Roles.Admin)]
        [HttpPost]
        public async Task<IActionResult> AddToRole(string userName, string role)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(x => x.UserName == userName);
            if (user != null)
            {
                var result = await userManager.AddToRoleAsync(user, role);
                if (result.Succeeded)
                {
                    return Ok($"{userName} added to {role} successfully");
                }
                else
                {
                    var errors = result.Errors.ToList();
                    return Content(String.Join(",", errors.Select(x => $"{x.Code}-{x.Description}")));
                }
            }

            return BadRequest($"No UserName exists for: {userName}");
        }

        [Authorize("ApiAndWebPolicy", Roles = Roles.Admin)]
        [HttpPost]
        public async Task<IActionResult> RemoveFromRole(string userName, string role)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(x => x.UserName == userName);
            if (user != null)
            {
                var result = await userManager.RemoveFromRoleAsync(user, role);
                if (result.Succeeded)
                {
                    return Ok($"{userName} removed from {role} successfully");
                }
                else
                {
                    var errors = result.Errors.ToList();
                    return Content(string.Join(",", errors.Select(x => $"{x.Code}-{x.Description}")));
                }
            }

            return BadRequest($"No UserName exists for: {userName}");
        }
    }
}
