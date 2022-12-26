using Blazorise;
using FF.Blazer.Pages.Games;
using FF.Domain.Enum;
using FF.Shared.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Json;

namespace FF.Blazer.Pages.Admin.Games
{
    public partial class CreateGame
    {
        private GameDto Game = new GameDto();
        CreateGameVm GameVm = new CreateGameVm();
        public bool IsBusy { get; private set; }
        Validations validationsCreateGameForm;
        private Dictionary<IBrowserFile, string> loadedFiles = new Dictionary<IBrowserFile, string>();
        private Dictionary<IBrowserFile, string> transliteFiles = new Dictionary<IBrowserFile, string>();
        private long maxFileSize = 1024 * 5000;
        private int maxAllowedFiles = 2;
        public bool IsLoading { get; private set; }
        string exceptionMessage;

        [Inject] INotificationService NotificationService { get; set; }

        Task ShowServerResult(string message, bool error)
        {
            if (error)
            {
                global::System.Console.WriteLine("returning error");
                return NotificationService.Error(message, "Error");
            }                
            else
                return NotificationService.Success(message, "Complete");
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        async Task LoadTransliteFile(InputFileChangeEventArgs e)
        {
            IsLoading = true;
            transliteFiles.Clear();
            exceptionMessage = string.Empty;
            Console.WriteLine("loading file");
            try
            {
                foreach (var file in e.GetMultipleFiles(maxAllowedFiles))
                {
                    using var reader = new StreamReader(file.OpenReadStream(maxFileSize));
                    transliteFiles.Add(file, await reader.ReadToEndAsync());
                }
            }
            catch (Exception ex)
            {
                exceptionMessage = ex.Message;
                Console.WriteLine(exceptionMessage);
            }

            IsLoading = false;
        }

        async Task LoadFiles(InputFileChangeEventArgs e)
        {
            IsLoading = true;
            loadedFiles.Clear();
            exceptionMessage = string.Empty;

            try
            {
                foreach (var file in e.GetMultipleFiles(maxAllowedFiles))
                {
                    using var reader =
                        new StreamReader(file.OpenReadStream(maxFileSize));

                    loadedFiles.Add(file, await reader.ReadToEndAsync());
                }
            }
            catch (Exception ex)
            {
                exceptionMessage = ex.Message;
            }

            IsLoading = false;
        }

        public async Task Submit()
        {
            if (await validationsCreateGameForm.ValidateAll())
            {                
                await validationsCreateGameForm.ClearAll();

                try
                {                    
                    //set the selected game types
                    var gameType = GameType.None;
                    foreach (var item in GameVm.SelectedGameTypes)
                    {
                        gameType |= item;
                    }
                    GameVm.GameType = gameType;

                    if(transliteFiles?.Count > 0)
                    {
                        var f = await transliteFiles?.ElementAt(0).Key.RequestImageFileAsync("image/jpeg", 400, 250).AsTask();
                        using (var stream = f.OpenReadStream(maxFileSize))
                        using (var ms = new MemoryStream())
                        {
                            //stream.Position = 0; //TODO: don't actually need to do this 
                            await stream.CopyToAsync(ms);
                            GameVm.TransliteImage = ms.ToArray();
                        }
                    }

                    IsBusy = true;
                    var response = await Http.PostAsJsonAsync("games/creategame", GameVm);
                    if (response.IsSuccessStatusCode)
                    {
                        await validationsCreateGameForm.ClearAll();
                        var id = await response.Content.ReadAsStringAsync();
                        await ShowServerResult($"New game {id} created!", false);
                    }
                    else
                    {
                        var msg = await response.Content.ReadAsStringAsync();
                        await ShowServerResult($"Failed to add game, {response.StatusCode}:{msg}", true);
                    }
                }
                catch (Exception ex)
                {
                    await ShowServerResult($"Exception, {ex.Message}", true);
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }
    }
}
