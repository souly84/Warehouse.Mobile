using System.Threading.Tasks;
using Xamarin.Forms;

namespace Warehouse.Mobile.Extensions
{
    public static class VisualElementExtensions
    {
        public static async Task FocusAsync(this VisualElement element, int timeoutInMilliseconds = 1000)
        {
            int timeInMilliseconds = 0;
            while (timeInMilliseconds < timeoutInMilliseconds)
            {
                if (element.Focus())
                {
                    break;
                }
                await Task.Delay(30).ConfigureAwait(true);
                timeInMilliseconds += 30;
            }
        }
    }
}
