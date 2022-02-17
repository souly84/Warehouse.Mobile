using System.Collections.Generic;
using Warehouse.Core;

namespace Warehouse.Mobile
{
    public class ReceptionGoodComparer : IComparer<IGoodConfirmation>
    {
        public int Compare(IGoodConfirmation x, IGoodConfirmation y)
        {
            if (x == null)
            {
                return y == null ? 0 : -1;
            }
            else
            {
                return y != null
                    ? Compare(x.Good, y.Good)
                    : 1;
            }
        }

        private int Compare(IReceptionGood x, IReceptionGood y)
        {
            if (x == null)
            {
                return y == null ? 0 : -1;
            }
            return y != null
                ? CompareUnknownAndExtraConfirmed(x, y)
                : 1;
        }

        private int CompareUnknownAndExtraConfirmed(IReceptionGood x, IReceptionGood y)
        {
            if (x.IsUnknown)
            {
                return y.IsUnknown ? 0 : 1;
            }

            if (x.IsExtraConfirmed)
            {
                return y.IsExtraConfirmed ? 0 : 1;
            }

            return y.IsExtraConfirmed ? -1 : 0;
        }
    }
}
