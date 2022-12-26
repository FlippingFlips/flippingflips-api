using FF.Core.Interface;
using FF.Core.Models;
using FF.Domain.Enum;
using FF.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PinMAME.NvMaps;
using PinMAME.NvMaps.Model;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

namespace FF.Core.Features.GamesPlayed.Cmd
{
    /// <summary>
    /// Adds a game played to database, saves scores. TODO: Maybe ReFactor out the different game posting types to keep the command slimmer, SOLID
    /// </summary>
    public class AddGamePlayedCmd : IRequest<GameAddedResult>
    {
        public AddGamePlayedCmd(GamePlayedDto gamePlayed)
        {
            GamePlayed = gamePlayed;
        }

        public GamePlayedDto GamePlayed { get; }
    }

    public class GameAddedResult
    {
        public bool Errored { get; set; }
        public string Message { get; set; }
    }

    internal class AddGamePlayedCmdHandler : IRequestHandler<AddGamePlayedCmd, GameAddedResult>
    {
        private readonly IRepository repository;
        private readonly ILogger<AddGamePlayedCmdHandler> logger;
        private GameAddedResult result;

        public AddGamePlayedCmdHandler(IRepository repository, ILogger<AddGamePlayedCmdHandler> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        void SetError(string msg)
        {
            result.Message = msg;
            result.Errored = true;
        }

        public async Task<GameAddedResult> Handle(AddGamePlayedCmd request, CancellationToken cancellationToken)
        {
            var opt = request.GamePlayed;
            result = new GameAddedResult();

            //find the game in progress and return error if none found
            var id = !string.IsNullOrWhiteSpace(opt.Id) ? opt.Id.Replace("\"", "") : string.Empty;
            var gip = await repository.GamesInProgress
                .AsNoTracking()
                .Include(g => g.Game)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (gip == null) SetError("Invalid game in progress id");
            if (result.Errored) return result;
            
            //create game played to save
            var gamePlayed = new GamePlayed()
            {
                ApplicationUserId = gip.ApplicationUserId,
                BallsPerGame = gip.BallsPerGame > 10 ? 10 : gip.BallsPerGame,
                Created = gip.Created,
                CRC = opt.CRC,
                Desktop = gip.Desktop,
                SystemVersion = gip.SystemVersion,
                GameId = gip.GameId,
                Ended = DateTime.Now
            };
            gamePlayed.GameTime = gamePlayed.Ended - gip.Created;

            //is the game pinmame?
            var isPinmame = gip.Game.GameType.HasFlag(GameType.PinMame);

            //use transaction to save
            using var transaction = repository.Database.BeginTransaction();
            try
            {
                //save the nvram, could be useful later on to get adjustments and audits for trophy's etc...              
                if (isPinmame && !string.IsNullOrWhiteSpace(opt.NvRamBase64))
                {
                    //TODO: Check nvram size. Some games are larger, like transformers. not sure if want to save these to Db
                    gamePlayed.NvRam = opt.NvRamBase64;
                }

                //Add each players score if available and the game in progress id isn't null                
                if (!isPinmame) //TODO: add separate method for posting nvram?
                {
                    if (opt.P1Score <= 0)
                    {
                        SetError("Player 1 score is 0 or less");
                        return result;
                    }

                    //save the game played
                    await repository.GamesPlayed.AddAsync(gamePlayed);
                    await repository.SaveChangesAsync(cancellationToken);

                    var score = new Models.Score()
                    {
                        Points = opt.P1Score,
                        PlayerId = gip.Player1Id,
                        GamePlayedId = gamePlayed.Id,
                    };
                    repository.Scores.Add(score);

                    if (opt.P2Score.HasValue && gip.Player2Id.HasValue)
                    {
                        score = new Models.Score() { Points = opt.P2Score.Value, PlayerId = gip.Player2Id.Value, GamePlayedId = gamePlayed.Id };
                        repository.Scores.Add(score);

                        if (opt.P3Score.HasValue && gip.Player3Id.HasValue)
                        {
                            score = new Models.Score() { Points = opt.P3Score.Value, PlayerId = gip.Player3Id.Value, GamePlayedId = gamePlayed.Id };
                            repository.Scores.Add(score);

                            if (opt.P4Score.HasValue && gip.Player4Id.HasValue)
                            {
                                score = new Models.Score() { Points = opt.P4Score.Value, PlayerId = gip.Player4Id.Value, GamePlayedId = gamePlayed.Id };
                                repository.Scores.Add(score);
                            }
                        }
                    }
                }
                else
                {
                    logger.LogInformation($"posting pinmame game...");
                    var game = await repository.Games.AsNoTracking().FirstOrDefaultAsync(x => x.Id == gip.GameId);
                    //logger.LogInformation($"NVRAM bytes {bytes.Length}");

                    //get the parent rom as this is where the mapping should be stored
                    var roms = await repository.PinMameRoms.AsNoTracking()
                        .Where(x => x.Id == game.PinmameRomId || x.ParentRom == game.PinmameRomId)
                        .ToListAsync();
                    var romMapping = roms.Where(x => x.NvMapJson != null).FirstOrDefault();
                    if (string.IsNullOrWhiteSpace(romMapping?.NvMapJson))
                    {
                        SetError("No mappings found for this pinmame table.");
                        return result;
                    }
                    else
                    {
                        List<ScoreResult> lastScores = null;
                        if (romMapping.NoLastScores) //rom doesn't support last scores
                        {
                            lastScores = new List<ScoreResult>();
                            if (opt.P1Score <= 0)
                            {
                                SetError("Needs a score from at least one player");
                                return result;
                            }

                            lastScores.Add(new ScoreResult { Score = opt.P1Score });
                            lastScores.Add(new ScoreResult { Score = opt.P2Score ?? 0 });
                            lastScores.Add(new ScoreResult { Score = opt.P3Score ?? 0 });
                            lastScores.Add(new ScoreResult { Score = opt.P4Score ?? 0 });
                        }
                        else //get scores from NVram
                        {
                            var nvRamMap = JsonSerializer.Deserialize<NvRamMap>(romMapping.NvMapJson, new JsonSerializerOptions
                            {
                                AllowTrailingCommas = true
                            });
                            byte[] bytes = Convert.FromBase64String(opt.NvRamBase64); //nvram bytes
                            var parseNVRam = new ParseNVRAM(nvRamMap, bytes);
                            lastScores = parseNVRam.GetLastScores().ToList();

                            try
                            {
                                var gameAdjustments = parseNVRam.GetStandardAdjustments();                                
                                if (gameAdjustments.BallsPerGame > 0)
                                    gamePlayed.BallsPerGame = gameAdjustments.BallsPerGame;
                                if (gameAdjustments.TiltWarnings > 0)
                                    gamePlayed.TiltWarnings = gameAdjustments.TiltWarnings;
                                if (gameAdjustments.MaxExtraBall > 0)
                                    gamePlayed.MaxExtraBall = gameAdjustments.MaxExtraBall;
                                if (gameAdjustments.BallSaveTime > 0)
                                    gamePlayed.BallSave = gameAdjustments.BallSaveTime;
                            }
                            catch (Exception ex)
                            {
                                logger.LogError("No game adjustments found. " + ex.Message);
                            }

                        }

                        int scoreIndex = 0;
                        if (lastScores?.Count() > 0)
                        {
                            //save the game played after we have retrieved the balls, tilts
                            await repository.GamesPlayed.AddAsync(gamePlayed);
                            await repository.SaveChangesAsync(cancellationToken);

                            logger.LogInformation($"NVRAM scores found...");
                            var scoreP1 = lastScores.ElementAt(scoreIndex);
                            var score = new Models.Score() { Points = scoreP1.Score, PlayerId = gip.Player1Id, GamePlayedId = gamePlayed.Id };
                            repository.Scores.Add(score);

                            scoreIndex++;
                            var scoreP2 = lastScores.ElementAt(scoreIndex) ?? null;
                            if (scoreP2?.Score > 0)
                            {
                                score = new Models.Score() { Points = scoreP2.Score, PlayerId = gip.Player2Id.Value, GamePlayedId = gamePlayed.Id };
                                repository.Scores.Add(score);

                                scoreIndex++;
                                var scoreP3 = lastScores.ElementAt(scoreIndex) ?? null;
                                if (scoreP3?.Score > 0)
                                {
                                    score = new Models.Score() { Points = scoreP3.Score, PlayerId = gip.Player3Id.Value, GamePlayedId = gamePlayed.Id };
                                    repository.Scores.Add(score);
                                    scoreIndex++;
                                    var scoreP4 = lastScores.ElementAt(scoreIndex) ?? null;
                                    if (scoreP4?.Score > 0)
                                    {
                                        score = new Models.Score() { Points = scoreP4.Score, PlayerId = gip.Player4Id.Value, GamePlayedId = gamePlayed.Id };
                                        repository.Scores.Add(score);
                                    }
                                }
                            }
                        }
                        else
                        {
                            SetError("ERROR: No scores found in NvRam");
                            return result;
                        }
                    }
                }
                
                repository.GamesInProgress.Remove(gip);
                await repository.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync();
                return result;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                logger.LogError(ex.ToString());
                SetError($"Errors while saving scores to database: {ex.Message} {ex.InnerException?.Message}");
                return result;
            }
        }
    }
}
