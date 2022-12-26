using FF.Core.Auth;
using FF.Core.Models;
using FF.Domain.Models;
using FF.Domain.Models.ViewModel;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace FF.Core.Features.Users.Cmd
{
    /// <summary>
    /// Creates a token from a users login
    /// </summary>
    public class UserTokenLoginCmd : IRequest<AuthResponseVm>
    {
        public UserTokenLoginCmd(UserDto userDto)
        {
            UserDto = userDto;
        }

        public UserDto UserDto { get; }
    }

    internal class UserTokenLoginCmdHandler : IRequestHandler<UserTokenLoginCmd, AuthResponseVm>
    {
        private readonly IConfiguration configuration;
        private readonly UserManager<ApplicationUser> userManager;

        public UserTokenLoginCmdHandler(IConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            this.configuration = configuration;
            this.userManager = userManager;
        }

        public async Task<AuthResponseVm> Handle(UserTokenLoginCmd request, CancellationToken cancellationToken)
        {
            var result = new AuthResponseVm();

            //check user name then check password and return error message if incorrect
            var user = await userManager.FindByNameAsync(request.UserDto.UserName);
            if (user == null || !await userManager.CheckPasswordAsync(user, request.UserDto.Password))
            {
                result.IsAuthSuccessful = false;
                result.ErrorMessage = "Invalid login attempt";
                return result;
            }

            //get users roles
            var userRoles = await userManager.GetRolesAsync(user);

            //create token
            int.TryParse(configuration["Jwt:ExpireMinutes"], out var expire);
            var token = AuthToken.GetAccessToken(configuration["Jwt:Key"], 
                configuration["Jwt:Issuer"], 
                configuration["Jwt:Audience"], 
                user.Id, user.UserName, user.Email, userRoles, expire);
            
            result.IsAuthSuccessful = true;
            result.Token = token;
            return result;
        }
    }
}
