using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediaPrint;
using Warehouse.Core;
using Warehouse.Mobile.Extensions;

namespace Warehouse.Mobile.Reception
{
    public class ReceptionWithExtraConfirmedGoods : IReception
    {
        private readonly ReceptionWithUnkownGoods _reception;
        private readonly int _defaultMaxQuantity;
        private readonly IList<IReceptionGood> _extraConfirmedGoods = new List<IReceptionGood>();

        public ReceptionWithExtraConfirmedGoods(ReceptionWithUnkownGoods reception)
            : this(reception, 1000)
        {
        }

        public ReceptionWithExtraConfirmedGoods(ReceptionWithUnkownGoods reception, int defaultMaxQuantity)
        {
            _reception = reception;
            _defaultMaxQuantity = defaultMaxQuantity;
        }

        public IEntities<IReceptionGood> Goods => new ComposedGoods(_reception.Goods, _extraConfirmedGoods);

        public Task ValidateAsync(IList<IGoodConfirmation> goodsToValidate)
        {
            return _reception.ValidateAsync(goodsToValidate);
        }

        public async Task<IReceptionGood> ByBarcodeAsync(string barcodeData)
        {
            var good = _extraConfirmedGoods.FirstOrDefault(x => x.Equals(barcodeData));
            if (good != null)
            {
                return good;
            }
            var goods = await _reception.Goods.ByBarcodeAsync(barcodeData);
            if (goods.Any())
            {
                if (await goods.AllAsync(async x => await x.ConfirmedAsync()))
                {
                    var extraGood = new ExtraConfirmedReceptionGood(goods.First(), _defaultMaxQuantity);
                    _extraConfirmedGoods.Add(extraGood);
                    return extraGood;
                }
            }
            return await _reception.ByBarcodeAsync(barcodeData, true);
        }

    }

    public class ExtraConfirmedReceptionGood : IReceptionGood
    {
        private readonly IReceptionGood _good;
        private readonly int _defaultMaxQuantity;
        private IGoodConfirmation? _confirmation;

        public ExtraConfirmedReceptionGood(IReceptionGood good, int defaultMaxQuantity)
        {
            _good = good;
            _defaultMaxQuantity = defaultMaxQuantity;
        }

        public int Quantity => _good.Quantity;

        public IGoodConfirmation Confirmation => _confirmation ?? (_confirmation = new GoodConfirmation(this, _defaultMaxQuantity));

        public override bool Equals(object? obj)
        {
            return _good.Equals(obj);
        }

        public override int GetHashCode()
        {
            return _good.GetHashCode();
        }

        public void PrintTo(IMedia media)
        {
            _good.PrintTo(media);
        }
    }
}
