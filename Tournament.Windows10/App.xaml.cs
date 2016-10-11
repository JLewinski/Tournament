using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Tournament.Windows10.Data;
using Tournament.Windows10.Services.SettingsServices;

using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml.Data;

namespace Tournament.Windows10
{
    /// Documentation on APIs used in this page:
    /// https://github.com/Windows-XAML/Template10/wiki
    [Bindable]
    sealed partial class App : Template10.Common.BootStrapper
    {
        public App()
        {
            InitializeComponent();
            SplashFactory = (e) => new Views.Splash(e);

            var settings = SettingsService.Instance;
            RequestedTheme = settings.AppTheme;
            CacheMaxDuration = settings.CacheMaxDuration;
            ShowShellBackButton = settings.UseShellBackButton;
        }

        public override async Task OnInitializeAsync(IActivatedEventArgs args)
        {
            using (var db = new ApplicationDbContext())
            {
                db.Database.Migrate();
            }

            await Task.CompletedTask;
        }

        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            // long-running startup tasks go here
            NavigationService.Navigate(typeof(Views.HomePage));
            await Task.CompletedTask;
        }
    }
}
