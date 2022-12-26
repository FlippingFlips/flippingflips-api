using FF.Core.Interface;
using FF.Shared.ViewModel.Menus;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PinMAME.NvMaps.Model;
using PinMAME.NvMaps;
using System.Text.Json;

namespace FF.Core.Features.GamesPlayed.Query
{
    /// <summary>
    /// Get detailed information about the game played rom settings
    /// </summary>
    public class GetGamePlayedSettingsQuery : IRequest<IEnumerable<RomMenuItemVm>>
    {
        public GetGamePlayedSettingsQuery(long gamePlayedId)
        {
            GamePlayedId = gamePlayedId;
        }

        public long GamePlayedId { get; }
    }

    internal class GetGamePlayedSettingsQueryHandler : IRequestHandler<GetGamePlayedSettingsQuery, IEnumerable<RomMenuItemVm>>
    {
        private readonly IRepository repository;

        public GetGamePlayedSettingsQueryHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<RomMenuItemVm>> Handle(GetGamePlayedSettingsQuery request, CancellationToken cancellationToken)
        {
            var romMapping = await repository.
                    GamesPlayed.AsNoTracking()
                    .Include(g => g.Game).ThenInclude(r => r.PinmameRom)
                    .Select(s => new { s.Id, s.NvRam, s.Game.PinmameRom.NvMapJson })
                    .FirstOrDefaultAsync(x => x.Id == request.GamePlayedId);

            if (romMapping != null)
            {
                try
                {
                    var nvRamMap = JsonSerializer.Deserialize<NvRamMap>(romMapping.NvMapJson, new JsonSerializerOptions
                    {
                        AllowTrailingCommas = true
                    });
                    byte[] bytes = Convert.FromBase64String(romMapping.NvRam); //nvram bytes
                    var parser = new ParseNVRAM(nvRamMap, bytes);                    
                    var items = parser.ExportAdjustmentsAndDefaults();

                    return items.Select(x => new RomMenuItemVm
                    {
                        DefaultValue = x.DefaultValue,
                        Key = x.Key,
                        Menu = x.Menu,
                        Name = x.Name,
                        Value = x.Value
                    });
                }
                catch
                {
                    //TODO: logging why this failed
                }
            }

            return null;
        }
    }
}
