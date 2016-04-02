using SCIPclient.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Windows.Storage;
using Windows.UI.Xaml.Navigation;

namespace SCIPclient.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {

        public LoginPageViewModel()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                //Value = "Designtime value";
            }
        }

        string _id = string.Empty;
        public string Id
        {
            get
            {
                return ApplicationData.Current.LocalSettings.Values.ContainsKey("Id") ? 
                    (string)ApplicationData.Current.LocalSettings.Values["Id"] : "";
            }
            set { Set(ref _id, value); }
        }

        string _pass = string.Empty;
        public string Pass
        {
            get
            {
                return ApplicationData.Current.LocalSettings.Values.ContainsKey("Pass") ?
                    (string)ApplicationData.Current.LocalSettings.Values["Pass"] : "";
            }
            set { Set(ref _pass, value); }
        }

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            if (state.Any())
            {
                Id=  state[nameof(Id)]?.ToString();
                Pass = state[nameof(Pass)]?.ToString();
                state.Clear();
            }
            return Task.CompletedTask;
        }

        public override Task OnNavigatedFromAsync(IDictionary<string, object> state, bool suspending)
        {
            if (suspending)
            {
                state[nameof(Id)] = Id;
                state[nameof(Pass)] = Pass;
            }
            return Task.CompletedTask;
        }

        public override Task OnNavigatingFromAsync(NavigatingEventArgs args)
        {
            args.Cancel = false;
            return Task.CompletedTask;
        }

        public void GotoSettings() =>
            NavigationService.Navigate(typeof(Views.SettingsPage), 0);

        public void GotoPrivacy() =>
            NavigationService.Navigate(typeof(Views.SettingsPage), 1);

        public void GotoAbout() =>
            NavigationService.Navigate(typeof(Views.SettingsPage), 2);

        public async void Login()
        {
            try
            {
                ApplicationData.Current.LocalSettings.Values["Id"] = _id;
                ApplicationData.Current.LocalSettings.Values["Pass"] = _pass;

                Shell.SetBusy(true, Id+" logging...");
                await Task.Delay(3000);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Shell.SetBusy(false);
            }
        }
    }
}
