using System.IO;
using System.Reflection;
using Xamarin.Forms;

namespace Warehouse.Mobile.Views
{
    public partial class MenuSelectionView : ContentPage
    {
        public MenuSelectionView()
        {
            try
            {
                InitializeComponent();
            }
            catch (FileNotFoundException)
            {
                // This trick is used here because of unit tests
                // Not all assemblies that referenced by the project implemented
                // for netcore3.1 project and not available during the tests run
            }
            catch (TargetInvocationException e) when (e.InnerException is FileNotFoundException)
            {
                // This trick is used here because of unit tests
                // Not all assemblies that referenced by the project implemented
                // for netcore3.1 project and not available during the tests run
            }
        }
    }
}
