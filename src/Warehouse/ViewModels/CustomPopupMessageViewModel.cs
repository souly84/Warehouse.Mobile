using System;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Warehouse.Mobile.Extensions;

namespace Warehouse.Mobile.ViewModels
{
    public class CustomPopupMessageViewModel : BindableBase, IInitialize
    {
        private readonly INavigationService _navigationService;
        private DelegateCommand? _actionCommand;
        private Action<bool, Exception?>? _callBack;

        public CustomPopupMessageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
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

        public void Initialize(INavigationParameters parameters)
        {
            Severity = parameters.Value<PopupSeverity>("Severity");
            Title = parameters.Value<string>("Title");
            Message = parameters.Value<string>("Message");
            ActionText = parameters.Value<string>("ActionText");
            _callBack = parameters.Value<Action<bool, Exception?>>("CallBack");
        }

        public DelegateCommand ActionCommand => _actionCommand ?? (_actionCommand = new DelegateCommand(async () =>
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
        }));
    }
}
