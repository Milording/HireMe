using ShowJet.Extensions;
using ShowJet.Models;
using ShowJet.Services;
using ShowJet.Services.Api;
using ShowJet.Services.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Wrangel.Mvvm;
using Wrangel.Mvvm.Commands;
using Wrangel.Services;

namespace ShowJet.ViewModels
{
    /// <summary>
    /// The view model for <see cref="ShowJet.Views.SignUpViaPhonePageView"/>
    /// </summary>
    public class SignUpViaPhonePageViewModel : ViewModelBase
    {
        #region Observable properties

        private string code;
        /// <summary>
        /// Gets or sets code from SMS.
        /// </summary>
        public string Code
        {
            get { return code; }
            set
            {
                this.SetProperty(ref code, value);
                this.ContinueCommand.RaiseCanExecuteChanged();
            }
        }

        private string name;
        /// <summary>
        /// Gets or sets user name.
        /// </summary>
        public string Name
        {
            get { return name; }
            set
            {
                this.SetProperty(ref name, value);
                this.ContinueCommand.RaiseCanExecuteChanged();
            }
        }

        private string phone;
        /// <summary>
        /// User's phone
        /// </summary>
        public string Phone
        {
            get { return phone; }
            set { this.SetProperty(ref phone, value); }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Gets the command to continue.
        /// </summary>
        public Command ContinueCommand { get; private set; }

        /// <summary>
        /// Gets the command to resend code.
        /// </summary>
        public Command ResendCodeCommand { get; private set; }

        #endregion

        #region Services

        /// <summary>
        /// The service for navigation between pages.
        /// </summary>
        private INavigationService navigationService;

        /// <summary>
        /// The service for working with API.
        /// </summary>
        private IShowJetApiService apiService;

        /// <summary>
        /// The service for working with app setings.
        /// </summary>
        private IShowJetSettingsService settingsService;

        /// <summary>
        /// The service for user notifications.
        /// </summary>
        private IShowJetNotificationsService notificationsService;

        /// <summary>
        /// The service for working with status bar.
        /// </summary>
        private IShowJetStatusBarService statusBarService;

        #endregion

        #region Contructors

        /// <summary>
        /// Creates the new instance of <see cref="SignUpViaPhoneViewModel"/> class.
        /// </summary>
        public SignUpViaPhonePageViewModel(INavigationService navigationService, IShowJetApiService apiService,
            IShowJetSettingsService settingsService, IShowJetNotificationsService notificationsService,
            IShowJetLocalizationService localizationService, IShowJetStatusBarService statusBarService)
        {
            this.navigationService = navigationService;
            this.apiService = apiService;
            this.settingsService = settingsService;
            this.notificationsService = notificationsService;
            this.statusBarService = statusBarService;
            this.ContinueCommand = new AsyncBlockableCommand(this.OnContinueCommand, this.CanExecuteContinueCommand);
            this.ResendCodeCommand = new AsyncBlockableCommand(this.OnResendCodeCommand);
        }

        #endregion

        #region Command handlers

        /// <summary>
        /// Invoked when given the command to load this page data.
        /// </summary>
        protected override void OnLoadCommand(NavigationContext navigationContext)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendView(this.GetType().Name.Replace("ViewModel", String.Empty));

            this.Phone = this.navigationService.GetNavigationInfo().Parameter as string;
        }

        /// <summary>
        /// Invoked when given the command to continue.
        /// </summary>
        /// <returns></returns>
        public async Task OnContinueCommand()
        {
            await this.statusBarService.RunWithProgressIndication(async()=> 
            {
                var tokenResponse = await apiService.PostUnpacked<String>(
                    new
                    {
                        device_id = this.settingsService.Get<string>(SettingsKeys.DeviceId),
                        phone = this.phone.ParsePhone(),
                        code = Code,
                        name = Name,
                    }, "auth/phone/sign_in.json ");

                if (tokenResponse.Status.IsError)
                {
                    if (tokenResponse.Status.HttpStatusCode == Windows.Web.Http.HttpStatusCode.Unauthorized)
                    {
                        await this.notificationsService.ShowErrorMessage("Неверный или просроченный код подтверждения.");
                        return;
                    }
                    else
                    {
                        await this.notificationsService.ShowErrorMessage(tokenResponse.Status.Error);
                        return;
                    }
                }

                this.settingsService.Set(SettingsKeys.AccessToken, tokenResponse.Content);
                var meResponse = await this.apiService.GetUnpacked<User>("my/me.json");

                if (meResponse.Status.IsError)
                {
                    await this.notificationsService.ShowErrorMessage(meResponse.Status.Error);
                    return;
                }

                this.settingsService.Set(SettingsKeys.User, meResponse.Content);
                this.navigationService.Navigate("Hooray");
            });            
        }

        /// <summary>
        /// Invoked when given the command to resend code.
        /// </summary>
        public async Task OnResendCodeCommand()
        {
            await this.statusBarService.RunWithProgressIndication(async ()=> 
            {
                var codeResponse = await apiService.Post<object>(new { phone = this.phone.ParsePhone() }, 
                    "auth/phone/code.json");

                if (codeResponse.Status.IsError)
                {
                    if (codeResponse.Status.HttpStatusCode == Windows.Web.Http.HttpStatusCode.TooManyRequests)
                    {
                        await this.notificationsService.ShowErrorMessage("Вы запрашиваете новый код слишком часто. Попробуйте позже.");
                        return;
                    }
                    else
                    {
                        await this.notificationsService.ShowErrorMessage(codeResponse.Status.Error);
                        return;
                    }
                }

                await this.notificationsService.ShowMessage("Новый код выслан на ваш телефонный номер.");
            });
        }

        /// <summary>
        /// Determines if the command to continue can be executed.
        /// </summary>
        /// <returns></returns>
        private bool CanExecuteContinueCommand()
        {
            return !String.IsNullOrWhiteSpace(this.code) && !String.IsNullOrWhiteSpace(this.name);
        }

        #endregion
    }
}
