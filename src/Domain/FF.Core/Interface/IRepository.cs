using FF.Core.Models;
using FF.Domain.Models.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FF.Core.Interface
{
    public interface IRepository
    {
        DbSet<CustomList> CustomLists { get; set; }
        DbSet<Game> Games { get; set; }
        DbSet<GameInProgress> GamesInProgress { get; set; }
        DbSet<GamePlayed> GamesPlayed { get; set; }
        DbSet<PinmameRom> PinMameRoms { get; set; }
        DbSet<Player> Players { get; set; }
        DbSet<Score> Scores { get; set; }
        DbSet<ApplicationUser> Users { get; set; }
        Microsoft.EntityFrameworkCore.Infrastructure.DatabaseFacade Database { get; }

        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        EntityEntry Update(object entity);
    }
}