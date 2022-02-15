using System;
using Dotnet.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Warehouse.Mobile.Extensions;

namespace Warehouse.Mobile.ViewModels
{
    public class CustomPopupMessageViewModel : BindableBase, IInitialize
    {
        private readonly INavigationService _navigationService;
        private Action<bool, Exception?>? _callBack;
        private readonly CachedCommands _commands;

        public CustomPopupMessageViewModel(
            ICommands commands,
            INavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _ = commands ?? throw new ArgumentNullException(nameof(commands));
            _commands = commands.Cached();
        }

        private PopupSeverity _severity;
        public PopupSeverity Severity
        {
            get => _severity;
            set => SetProperty(ref _severity, value);
        }

        private string _title = string.Empty;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _message = string.Empty;
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        private string _actionText = string.Empty;
        public string ActionText
        {
            get => _actionText;
            set => SetProperty(ref _actionText, value);
        }

        public IAsyncCommand ActionCommand => _commands.AsyncCommand(async () =>
        {
            try
            {
                await _navigationService.GoBackAsync();
                _callBack?.Invoke(true, null);
            }
            catch (Exception ex)
            {
                _callBack?.Invoke(false, ex);
            }
        });

        public void Initialize(INavigationParameters parameters)
        {
            Severity = parameters.Value<PopupSeverity>("Severity");
            Title = parameters.Value<string>("Title");
            Message = parameters.Value<string>("Message");
            ActionText = parameters.Value<string>("ActionText");
            _callBack = parameters.Value<Action<bool, Exception?>>("CallBack");
        }
    }
}
