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

        public IEntities<IReceptionGood> Goods => new ListOfEntities<IReceptionGood>(
            new MockReceptionGood("1", 1, "1111")
        );

        public Task ValidateAsync(IList<IGoodConfirmation> goodsToValidate)
        {
            throw _exception;
        }
    }
}
