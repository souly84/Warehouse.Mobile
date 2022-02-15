using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Warehouse.Core;

namespace Warehouse.Mobile.ViewModels
{
    public class ReceptionGroup : ObservableCollection<ReceptionGoodViewModel>
    {
        private readonly IReception _reception;
        private readonly int _originalCount;

        public ReceptionGroup(
            IReception reception,
            IEnumerable<ReceptionGoodViewModel> goods)
            : base(goods)
        {
            _reception = reception;
            _originalCount = this.Count;
        }

        public string Name => $"Reception {_reception.Id}";

        public string TotalCount => $"{this.Count(x => x.Regular)}/{_originalCount}";

        public Task CommitAsync()
        {
            return _reception.Confirmation().CommitAsync();
        }

        public Task<IList<IReceptionGood>> ByBarcodeAsync(
            string barcodeData,
            bool ignoreConfirmed = false)
        {
            return _reception.ByBarcodeAsync(barcodeData, ignoreConfirmed);
        }
    }
}
