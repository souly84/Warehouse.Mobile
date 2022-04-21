using Xamarin.Forms;

namespace Warehouse.Mobile.Plugins
{
    public static class PageExtensions
    {
        public static T? FromHistory<T>(this Page page)
            where T : Page
        {
            if (page is NavigationPage navigationPage)
            {
                foreach (var historyPage in navigationPage.Navigation.NavigationStack)
                {
                    if (historyPage is T)
                    {
                        return (T)historyPage;
                    }
                }
            }
            return default;
        }
    }
}
