using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Dotnet.Commands;
using MediaPrint;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using Warehouse.Core;
using Warehouse.Core.Plugins;
using Warehouse.Mobile.Extensions;

namespace Warehouse.Mobile.ViewModels
{
    public class HistoryViewModel: BindableBase, IInitializeAsync
    {
        private readonly IPageDialogService _dialog;
        private string? _supplierName;
        private ObservableCollection<ReceptionGroup>? _receptionGoods;
        private readonly INavigationService _navigationService;
        private readonly CachedCommands _cachedCommands;
        private readonly ICommands _commands;
        private readonly IKeyValueStorage _keyValueStorage;
        private int _originalCount;


        public HistoryViewModel(
            IPageDialogService dialog,
            INavigationService navigationService,
            ICommands commands,
            IKeyValueStorage keyValueStorage)
        {
            _navigationService = navigationService;
            _commands = commands;
            _dialog = dialog;
            _cachedCommands = commands.Cached();
            _keyValueStorage = keyValueStorage;
        }

        public ObservableCollection<ReceptionGroup> ReceptionGoods
        {
            get => _receptionGoods ?? new ObservableCollection<ReceptionGroup>();
            set => SetProperty(ref _receptionGoods, value);
        }

        public string? SupplierName
        {
            get => _supplierName;
            set => SetProperty(ref _supplierName, value);
        }

        public async Task InitializeAsync(INavigationParameters parameters)
        {
            try
            {
                var supplier = parameters.Value<ISupplier>("Supplier");
                ReceptionGoods = await supplier.ReceptionViewModelsAsync(_commands, _keyValueStorage);
                _originalCount = ReceptionGoods.Sum(r => r.Count);
                SupplierName = supplier.ToDictionary().ValueOrDefault<string>("Name");
            }
            catch (Exception ex)
            {
                await _dialog.DisplayAlertAsync("Error", ex.ToString(), "Ok");
            }
        }
    }
}
