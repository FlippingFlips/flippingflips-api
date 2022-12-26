using FF.Api.Data.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FF.Api.Dto;
using System.Text;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authorization;
using FF.Infrastructure.Data;
using FF.Domain.Models;
using FF.Core.Models;
using FF.Core.Features.Users.Cmd;
using FF.Api.Base;
using MediatR;

namespace FF.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public partial class AccountController : FlipsApiControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ILogger<AccountController> logger;
        private readonly IConfiguration config;
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IEmailSender emailSender;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, 
            ILogger<AccountController> logger, IConfiguration config,
            ApplicationDbContext applicationDbContext, IEmailSender emailSender, IMediator mediator) : base(mediator)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.logger = logger;
            this.config = config;
            this.applicationDbContext = applicationDbContext;
            this.emailSender = emailSender;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [Authorize("ApiAndWebPolicy", Roles = Roles.Admin)]
        [HttpPost("Register")]
        public async Task<IActionResult> CreateAccount(string returnUrl, string callBackUrl, [FromBody] RegisterModel inputModel)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                var existingUser = await userManager.FindByNameAsync(inputModel.Username);
                if (existingUser != null)
                {
                    StatusMessage = "Username already exists";
                    return RedirectToPage("/");
                }

                //find existing user by email, return if email already in use
                existingUser = await userManager.FindByEmailAsync(inputModel.Email);
                if (existingUser != null) return BadRequest("Email already exists");

                //Create user model and set EmailConfirmed if email confirmations are switched off, see appSettings, RequireConfirmedAccount
                var user = new ApplicationUser { UserName = inputModel.Username, Email = inputModel.Email, 
                    MachName = inputModel.MachineName, 
                    Country = inputModel.Country, PlayersPerCabinet = 10, 
                    //set email confirmed if not set to require confirmation
                    EmailConfirmed = !userManager.Options.SignIn.RequireConfirmedAccount ? true : false };

                var result = await userManager.CreateAsync(user, inputModel.Password);
                if (result.Succeeded)
                {
                    logger.LogInformation("User created a new account with password.");
                    //create default player
                    try
                    {
                        await applicationDbContext.Players.AddAsync(new Player() { Created = DateTime.Now, Initials = inputModel.Initials, MachineDefault = true, ApplicationUserId = user.Id, Name = inputModel.Name });
                        await applicationDbContext.SaveChangesAsync();
                    }
                    catch { this.logger.LogError("Failed to create player..."); }

                    //add user to standard user role, these roles for a user can be changed via API, add / remove role
                    await userManager.AddToRoleAsync(user, Roles.User);

                    //user isn't email confirmed so we should get them to confirm it
                    if (!user.EmailConfirmed)
                    {
                        //generate email code to confirm with
                        var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                        //old method to post to this server
                        //var callbackUrl = Url.Page( "/Account/ConfirmEmail", pageHandler: null, values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl }, protocol: Request.Scheme);                        

                        if (userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            //create callback url with query string with userId and code
                            callBackUrl += $"?userId={user.Id}&code={code}";

                            //send the user an email to confirm their account
                            await emailSender.SendEmailAsync(inputModel.Email, "Confirm your email to complete the process", $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callBackUrl)}'>clicking here</a>.");

                            return RedirectToPage("RegisterConfirmation", new { email = inputModel.Email, returnUrl = returnUrl });
                        }
                        else
                        {
                            await signInManager.SignInAsync(user, isPersistent: false);
                            return LocalRedirect(returnUrl);
                        }
                    }

                    return Ok("User created");
                }

                //add errors and return them in bad request
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed
            return BadRequest(ModelState);
        }

        /// <summary>
        /// Creates a bearer token login from userName and Password
        /// </summary>
        /// <param name="userForAuthentication"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserDto userForAuthentication)
        {
            var cmd = new UserTokenLoginCmd(userForAuthentication);
            var result = await mediator.Send(cmd);

            if (!result.IsAuthSuccessful)
                return Unauthorized(result);

            return Ok(result);
        }
    }
}