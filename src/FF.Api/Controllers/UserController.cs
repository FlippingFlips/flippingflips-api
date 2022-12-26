using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FF.Core.Extensions;
using FF.Api.Base;
using MediatR;
using FF.Core.Features.Machines.Query;
using FF.Core.Features.Users.Cmd;
using FF.Shared.Model;

namespace FlippingFlips.Blazor.Server.Controllers
{
    [Authorize("ApiAndWebPolicy")]
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController : FlipsApiControllerBase
    {
        public UserController(IMediator mediator = null) : base(mediator) { }

        /// <summary>
        /// Gets the logged in users machine and players
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetUserMachineAsync()
        {            
            try
            {
                var userId = User.Identity.GetUserId();
                var q = new GetUserMachinesQuery(new UserMachinesQueryDto() { UserId = userId, IncludePlayers = true});
                return Ok(await mediator.Send(q));
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message} {ex.InnerException?.Message}");
            }
        }

        /// <summary>
        /// Gets the logged in users machine and players
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetUserMachineByUsernameAsync(string userName)
        {
            try
            {
                var q = new GetUserMachinesQuery(new UserMachinesQueryDto() { UserName = userName, IncludePlayers = true });
                return Ok(await mediator.Send(q));
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message} {ex.InnerException?.Message}");
            }
        }

        /// <summary>
        /// Returns users machines
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetUserMachinesAsync(UserMachinesQueryDto machinesQueryDto)
        {
            try
            {
                if (machinesQueryDto == null)
                    machinesQueryDto = new UserMachinesQueryDto();
                //explicit set to null to prevent unauthorized. User GetUserMachineAsync
                machinesQueryDto.UserId = null;
                var q = new GetUserMachinesQuery(machinesQueryDto);
                return Ok(await mediator.Send(q));
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message} {ex.InnerException?.Message}");
            }
        }

        /// <summary>
        /// Generate API Key so the users machine can post scores with it
        /// </summary>
        /// <returns></returns>        
        [HttpGet]
        public async Task<IActionResult> GenerateApiKeyAsync()
        {
            try
            {
                //get the logged in users id
                var userId = User.Identity.GetUserId();

                //generate new key, save to database and return it
                var cmd = new GenerateUserApiKeyCmd(userId);
                return Ok(await mediator.Send(cmd));                
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message} {ex.InnerException?.Message}");
            }
        }
    }
}
