using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warehouse.Core;
using Warehouse.Core.Plugins;
using Warehouse.Mobile.Extensions;

namespace Warehouse.Mobile.Reception.ViewModels
{
    public class SupplierMutlipleReception : IReception
    {
        private readonly ISupplier _supplier;
        private readonly IKeyValueStorage _keyValueStorage;

        public SupplierMutlipleReception(ISupplier supplier, IKeyValueStorage keyValueStorage)
        {
            _supplier = supplier;
            _keyValueStorage = keyValueStorage;
        }

        public string Id => throw new NotSupportedException("Not possible to get the id for multiple receptions");

        private IReceptionGoods _goods;
        public IReceptionGoods Goods
        {
            get
            {
                if (_goods == null)
                {
                    _goods = new CombinedSupplierGoods(_supplier, _keyValueStorage).Cached();
                }
                return _goods;
            }
        }

        public async Task<IList<IReceptionGood>> ByBarcodeAsync(string barcodeData, bool ignoreConfirmed = false)
        {
            var receptions = await _supplier.Receptions.ToListAsync();
            foreach (var reception in receptions)
            {
                var goods = await reception.ByBarcodeAsync(barcodeData, ignoreConfirmed);
                if (goods.Any() && !goods.Any(x => x.IsUnknown))
                {
                    return goods;
                }
            }
            if (receptions.Any())
            {
                return await receptions.First().ByBarcodeAsync(barcodeData, ignoreConfirmed);
            }
            return new List<IReceptionGood>();
        }

        public async Task ValidateAsync(IList<IGoodConfirmation> goodsToValidate)
        {
            var receptions = await _supplier.Receptions.ToListAsync();
            foreach (var rec in receptions)
            {
                var goods = await rec.Goods.ToListAsync();
                var receptionGoods = new List<IGoodConfirmation>();
                foreach (var goodsToV in goodsToValidate)
                {
                    if (goods.Contains(goodsToV.Good))
                    {
                        receptionGoods.Add(goodsToV);
                    }
                }
                if (receptionGoods.Any())
                {
                    await rec.ValidateAsync(receptionGoods);
                }
            }
        }
    }

    public class CombinedSupplierGoods : IReceptionGoods
    {
        private readonly ISupplier _supplier;
        private readonly IKeyValueStorage _keyValueStorage;

        public CombinedSupplierGoods(ISupplier supplier, IKeyValueStorage keyValueStorage)
        {
            _supplier = supplier;
            _keyValueStorage = keyValueStorage;
        }

        public async Task<IList<IReceptionGood>> ToListAsync()
        {
            var supplierReceptions = await _supplier.Receptions.ToListAsync();
            var result = new List<IReceptionGood>();
            foreach (var suppl in supplierReceptions)
            {
                var reception = suppl
                .WithExtraConfirmed()
                .WithoutInitiallyConfirmed()
                .WithConfirmationProgress(_keyValueStorage);
                result.AddRange(await reception.Goods.ToListAsync());
            }
            return result;
            
        }

        public IReceptionGood UnkownGood(string barcode)
        {
            return _supplier.Receptions.FirstAsync().RunSync().Goods.UnkownGood(barcode);
        }

        public IEntities<IReceptionGood> With(IFilter filter)
        {
            throw new NotImplementedException();
        }
    }
}
