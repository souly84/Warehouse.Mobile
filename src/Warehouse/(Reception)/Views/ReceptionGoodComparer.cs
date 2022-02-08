using System;
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
                if (y == null)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                if (y == null)
                {
                    return 1;
                }
                else
                {
                    return Compare(x.Good, y.Good);
                }
            }
        }

        private int Compare(IReceptionGood x, IReceptionGood y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                if (y == null)
                {
                    return 1;
                }
                else
                {
                    if (x.IsUnknown && y.IsUnknown)
                    {
                        if (x.IsExtraConfirmed && y.IsExtraConfirmed)
                        {
                            return 0;
                        }
                        else
                        {
                            if (y.IsExtraConfirmed)
                            {
                                return -1;
                            }
                            else
                            {
                                return 1;
                            }
                        }
                    }
                    else
                    {
                        return 1;
                    }
                }
            }
        }
    }
}
