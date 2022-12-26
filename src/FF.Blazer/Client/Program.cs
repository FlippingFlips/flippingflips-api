using Blazored.LocalStorage;
using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using FF.Shared.Interface;
using FF.Shared.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace FF.Blazer.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddHttpClient("FF.Api", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            // Supply HttpClient instances that include access tokens when making requests to the server project
            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("FF.Api"));

            builder.Services.AddApiAuthorization();

            builder.Services.AddScoped<IFlipsClientService, FlipsClientService>();

            builder.Services.AddBlazoredLocalStorage();

            AddBlazorise(builder.Services);

            await builder.Build().RunAsync();
        }

        public static void AddBlazorise(IServiceCollection services)
        {
            services
                .AddBlazorise(o => {
                    o.IconStyle = IconStyle.Solid;
            });
            services
                .AddBootstrap5Providers()
                .AddFontAwesomeIcons();
        }
    }
}