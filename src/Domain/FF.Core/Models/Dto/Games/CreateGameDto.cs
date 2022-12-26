using FF.Core.Mappings;
using FF.Shared.Model;

namespace FF.Core.Models.Dto.Games
{
    public class CreateGameDto : Game, IMapFrom<CreateGameVm>
    {
        public byte[] TransliteImage { get; set; }
    }
}
