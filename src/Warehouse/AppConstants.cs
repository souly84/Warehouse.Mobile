using System;
using Warehouse.Mobile.Views;

namespace Warehouse.Mobile
{
    public static class AppConstants
    {
        //Pages
        public const string SupplierViewId = nameof(SelectSupplierView);
        public const string ReceptionDetailsViewId = nameof(ReceptionDetailsView);
        public const string PutAwayViewId = nameof(PutAwayView);
        public const string QuantityToMovePopupViewId = nameof(QuantityToMovePopupView);
        public const string StockMoveViewId = nameof(StockMoveView);
        public const string CustomPopupMessageViewId = nameof(CustomPopupMessageView);
        public static string Uri { get => "http://wdc-logcnt.eurocenter.be/webservice/api.php"; }
    }
}
