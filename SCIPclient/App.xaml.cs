using System;
using Windows.UI.Xaml;
using System.Threading.Tasks;
using SCIPclient.Services.SettingsServices;
using Windows.ApplicationModel.Activation;
using Microsoft.WindowsAzure.MobileServices;

namespace SCIPclient
{
    /// Documentation on APIs used in this page:
    /// https://github.com/Windows-XAML/Template10/wiki

    sealed partial class App : Template10.Common.BootStrapper
    {
        public static MobileServiceClient Client { get; private set; }
        public App()
        {
            Microsoft.ApplicationInsights.WindowsAppInitializer.InitializeAsync("15a40aec-ef48-471f-b0ad-be1735f11fea",
                Microsoft.ApplicationInsights.WindowsCollectors.Metadata |
                Microsoft.ApplicationInsights.WindowsCollectors.Session);
            InitializeComponent();
            SplashFactory = (e) => new Views.Splash(e);

            #region App settings

            var _settings = SettingsService.Instance;
            RequestedTheme = _settings.AppTheme;
            CacheMaxDuration = _settings.CacheMaxDuration;
            ShowShellBackButton = _settings.UseShellBackButton;

            #endregion

            Client = new MobileServiceClient("http://localhost:59992");
        }

        // runs even if restored from state
        public override Task OnInitializeAsync(IActivatedEventArgs args)
        {
            // content may already be shell when resuming
            if ((Window.Current.Content as Views.Shell) == null)
            {
                // setup hamburger shell
                var nav = NavigationServiceFactory(BackButton.Attach, ExistingContent.Include);
                Window.Current.Content = new Views.Shell(nav);
            }
            return Task.CompletedTask;
        }

        // runs only when not restored from state
        public override Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            NavigationService.Navigate(typeof(Views.LoginPage));
            return Task.CompletedTask;
        }
    }
}

