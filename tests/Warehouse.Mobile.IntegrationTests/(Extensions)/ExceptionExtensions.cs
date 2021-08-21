using System;

namespace Warehouse.Mobile.IntegrationTests
{
    public static class ExceptionExtensions
    {
        public static bool Contains<T>(this Exception ex)
        {
            while (ex != null)
            {
                if  (ex is T)
                {
                    return true;
                }
                ex = ex.InnerException;
            }
            return false;
        }
    }
}
