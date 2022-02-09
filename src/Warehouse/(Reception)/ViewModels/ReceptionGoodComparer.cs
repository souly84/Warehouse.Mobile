using System.Collections.Generic;
using Warehouse.Core;

namespace Warehouse.Mobile.Reception.Views
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
            if (x.IsUnknown && y.IsUnknown)
            {
                if (x.IsExtraConfirmed && y.IsExtraConfirmed)
                {
                    return 0;
                }
                else
                {
                    return y.IsExtraConfirmed ? -1 : 1;
                }
            }

            return 1;
        }
    }
}
