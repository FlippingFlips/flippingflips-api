using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using System.Reflection;
using FF.Domain.Models.Data;
using FF.Core.Models;
using FF.Core.Interface;
using Microsoft.Extensions.Configuration;
using AutoMapper;

namespace FF.Infrastructure.Data
{
    public partial class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>, IRepository
    {
        public DbSet<CustomList>? CustomLists { get; set; }
        public DbSet<Game>? Games { get; set; }
        public DbSet<GameInProgress>? GamesInProgress { get; set; }
        public DbSet<GamePlayed>? GamesPlayed { get; set; }
        public DbSet<Player>? Players { get; set; }
        public DbSet<PinmameRom>? PinMameRoms { get; set; }
        public DbSet<Score>? Scores { get; set; }

        private readonly IConfiguration configuration;
        private readonly IMapper mapper;

        public ApplicationDbContext(
                DbContextOptions options,
                IOptions<OperationalStoreOptions> operationalStoreOptions, IConfiguration configuration, 
                IMapper mapper) : base(options, operationalStoreOptions)
        {
            this.configuration = configuration;
            this.mapper = mapper;
        }

        /// <summary>
        /// Seeds users, roles and players from appsettings.json
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //get default values to seed to database
            var dbOptions = configuration.GetSection("DatabaseModelCreationOptions").Get<DatabaseModelCreationOptions>();
            if (dbOptions == null)
                throw new NullReferenceException($"No {nameof(DatabaseModelCreationOptions)} settings found in appsettings.json");

            //apply all configurations "/config/TEntityConfig"
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            //Create roles if they do not exist in the database
            if (dbOptions.UserRoles != null)
            {
                foreach (var role in dbOptions.UserRoles)
                {
                    builder.Entity<IdentityRole>().HasData(role);
                }
            }

            //add the users, roles and players
            if (dbOptions.Users != null)
            {
                var passHasher = new PasswordHasher<ApplicationUser>();
                int playerId = 1;
                foreach (var user in dbOptions.Users)
                {
                    user.Created = DateTime.Now;
                    user.MachBirthday = DateTime.Now;
                    user.PasswordHash = passHasher.HashPassword(new ApplicationUser(), user.Password);                    
                    user.NormalizedUserName = user.UserName.ToUpper();
                    user.NormalizedEmail = user.Email.ToUpper();                    

                    ApplicationUser appUser = mapper.Map<ApplicationUser>(user);
                    builder.Entity<ApplicationUser>().HasData(appUser);

                    //add a player for the user
                    builder.Entity<Player>().HasData(new Player
                    {
                        Id = playerId,
                        ApplicationUserId = user.Id,
                        Created = DateTime.UtcNow,
                        Initials = user.PlayerInitials,
                        Name = user.Player,
                        MachineDefault = true
                    });

                    //add the user to a role
                    string roleId = string.Empty;
                    roleId = dbOptions.UserRoles?.FirstOrDefault(x => x.Name == user.RoleName)?.Id ?? string.Empty;
                    if (!string.IsNullOrWhiteSpace(roleId))
                    {
                        //adds the user to a role
                        builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
                        {
                            RoleId = roleId,
                            UserId = user.Id
                        });
                    }

                    playerId++;
                }
            }           
        }
    }
}
