using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Warehouse.Core;

namespace Warehouse.Mobile.UnitTests.Mocks
{
    public class ValidateExceptionReception : IReception
    {
        private readonly Exception _exception;

        public ValidateExceptionReception(Exception exception)
        {
            _exception = exception;
        }

        public IReceptionGoods Goods => new MockReceptionGoods(
              new MockReceptionGood("1", 1, "1111")
        );

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj)
                || obj is DateTime;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_exception, Goods);
        }

        public Task ValidateAsync(IList<IGoodConfirmation> goodsToValidate)
        {
            throw _exception;
        }
    }
}
