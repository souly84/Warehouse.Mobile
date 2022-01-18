using System.IO;
using Xamarin.Forms;

namespace Warehouse.Mobile.Views
{
    public partial class ReceptionDetailsView : ContentPage
    {
        public ReceptionDetailsView()
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
        }
    }
}
