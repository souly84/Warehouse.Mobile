using System;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Warehouse.Mobile.Extensions;

namespace Warehouse.Mobile.ViewModels
{
    public class CustomPopupMessageViewModel : BindableBase, IInitialize
    {
        private readonly INavigationService _navigationService;

        public CustomPopupMessageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        private PopupSeverity _severity;
        public PopupSeverity Severity
        {
            get => _severity;
            set => SetProperty(ref _severity, value);
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _message;
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        private string _actionText;
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
        }

        private DelegateCommand _actionCommand;

        public DelegateCommand ActionCommand => _actionCommand ?? (_actionCommand = new DelegateCommand(async () =>
        {
            await _navigationService.GoBackAsync();
        }));
    }
}
