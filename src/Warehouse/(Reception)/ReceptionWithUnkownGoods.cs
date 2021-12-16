using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warehouse.Core;

namespace Warehouse.Mobile.Reception
{
    public class ReceptionWithUnkownGoods : IReception
    {
        private readonly IReception _reception;

        private IList<IReceptionGood> _unknownGoods = new List<IReceptionGood>();

        public ReceptionWithUnkownGoods(IReception reception)
        {
            _reception = reception;
        }

        public IEntities<IReceptionGood> Goods => new ComposedGoods(_reception.Goods, _unknownGoods);

        public Task ValidateAsync(IList<IGoodConfirmation> goodsToValidate)
        {
            return _reception.ValidateAsync(goodsToValidate);
        }

        public async Task<IReceptionGood> ByBarcodeAsync(string barcodeData)
        {
            var good = _unknownGoods.FirstOrDefault(x => x.Equals(barcodeData));
            if (good != null)
            {
                return good;
            }
            var goods = await _reception.Goods.ByBarcodeAsync(barcodeData);
            if (goods.Any())
            {
                return goods.First();
            }
            good = new MockReceptionGood("", 1000, barcodeData);
            _unknownGoods.Add(good);
            return good;
        }
    }

    public class ComposedGoods : IEntities<IReceptionGood>
    {
        private readonly IEntities<IReceptionGood> _original;
        private readonly IList<IReceptionGood> _unkownGoods;

        public ComposedGoods(IEntities<IReceptionGood> original, IList<IReceptionGood> unkownGoods)
        {
            _original = original;
            _unkownGoods = unkownGoods;
        }


        public async Task<IList<IReceptionGood>> ToListAsync()
        {
            var result = new List<IReceptionGood>(_unkownGoods);
            result.AddRange(await _original.ToListAsync());
            return result;
        }

        public IEntities<IReceptionGood> With(IFilter filter)
        {
            return new ComposedGoods(_original.With(filter), _unkownGoods);
        }
    }
}
