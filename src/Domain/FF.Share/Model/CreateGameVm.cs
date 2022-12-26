using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FF.Domain.Enum;
using FF.Shared.Attributes;

namespace FF.Shared.Model
{
    public class CreateGameVm
    {
        [Required]
        [StringLength(maximumLength: 100, ErrorMessage = "Title must be less than {0} chars", MinimumLength = 3)]
        public string Title { get; set; }

        [Required]
        [StringLength(maximumLength: 250, ErrorMessage = "Description must be less than {0} chars", MinimumLength = 3)]
        public string Description { get; set; }

        [Required]
        [YearRange(ErrorMessage = "Year must be greater than 1910")]
        public int Year { get; set; }

        [Required]
        [Range(1, 8, ErrorMessage = "Players must be between {0} and {1}")]
        public int Players { get; set; } = 4;

        [Required]
        [StringLength(maximumLength: 100, ErrorMessage = "Authors must be less than {0} chars", MinimumLength = 3)]
        public string Author { get; set; }

        [Required]
        [StringLength(maximumLength: 150, ErrorMessage = "FileUrl must be less than {0} chars", MinimumLength = 3)]
        public string FileUrl { get; set; }

        [Required]
        [StringLength(maximumLength: 150, ErrorMessage = "FileName must be less than {0} chars", MinimumLength = 3)]
        public string FileName { get; set; }

        [Required]
        [StringLength(maximumLength: 10, ErrorMessage = "Version length longer than {0} and less than {1}{", MinimumLength = 2)]
        public string Version { get; set; }

        [Required]
        [StringLength(maximumLength: 8, ErrorMessage = "CRC length must be {0} chars long", MinimumLength = 8)]
        public string CRC { get; set; }

        [StringLength(maximumLength: 8, ErrorMessage = "CRC length must be {0} chars long", MinimumLength = 8)]
        public string CRCPatched { get; set; }

        [StringLength(maximumLength: 150, ErrorMessage = "Patched FileName must be less than {0} chars", MinimumLength = 3)]
        public string FileNamePatched { get; set; }

        [StringLength(maximumLength: 200, ErrorMessage = "FilePatchUrl must be less than {0} chars", MinimumLength = 3)]
        public string FilePatchUrl { get; set; }

        [StringLength(maximumLength: 64, ErrorMessage = "Rom name must be less than {0} chars")]
        public string RomTitle { get; set; }

        [StringLength(maximumLength: 64, ErrorMessage = "Rom name must be less than {0} chars")]
        public string PinmameRomId { get; set; }

        [StringLength(maximumLength: 64, ErrorMessage = "Parent rom name must be less than {0} chars")]
        public string ParentRom { get; set; }

        public bool IsEnabled { get; set; }
        public string Manufacturer { get; set; }

        [Required]
        public GameType GameType { get; set; }

        [Required]
        public GameSystem GameSystem { get; set; }

        public IReadOnlyList<GameType> SelectedGameTypes { get; set; }
        public GameSystem SelectedGameSystem { get; set; }

        public List<GameTypeSelect> GameTypes { get; set; }
        public List<GameSystemSelect> GameSystems { get; set; }
        public byte[] TransliteImage { get; set; }

        public CreateGameVm()
        {
            GameTypes = new List<GameTypeSelect>();
            foreach (var item in Enum.GetValues(typeof(GameType)))
            {
                GameTypes.Add(new GameTypeSelect
                {
                    Key = item.ToString(),
                    Value = (GameType)Enum.Parse(typeof(GameType), item.ToString())
                });
            }
            GameSystems = new List<GameSystemSelect>();
            foreach (var item in Enum.GetValues(typeof(GameSystem)))
            {
                GameSystems.Add(new GameSystemSelect
                {
                    Key = item.ToString(),
                    Value = (GameSystem)Enum.Parse(typeof(GameSystem), item.ToString())
                });
            }
        }
    }
}
