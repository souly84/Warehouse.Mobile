using System.Collections.Generic;
using System.Linq;
using Rg.Plugins.Popup.Pages;
using Warehouse.Mobile.ViewModels;
using Xamarin.Forms;
using static Warehouse.Mobile.Tests.MockPageDialogService;

namespace Warehouse.Mobile.UnitTests.Extensions
{
    public static class PageExtensions
    {
        public static T ViewModel<T>(this Page page)
        {
            return (T)page.BindingContext;
        }

        public static List<DialogPage> ToDialogPages(this IEnumerable<PopupPage> pages)
        {
            return pages
                .Select(page => page.ToDialogPage())
                .ToList();
        }

        public static DialogPage ToDialogPage(this PopupPage page)
        {
            var viewModel = page.ViewModel<CustomPopupMessageViewModel>();
            return new DialogPage
            {
                Title = viewModel.Title,
                Message = viewModel.Message,
                CancelButton = viewModel.ActionText
            };
        }
    }
}
