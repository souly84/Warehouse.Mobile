using System.IO;
using System.Reflection;
using Warehouse.Mobile.ViewModels;
using Xamarin.Forms;
using System.Collections.Specialized;
using System.Threading.Tasks;

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
            catch (TargetInvocationException e) when (e.InnerException is FileNotFoundException)
            {
                // This trick is used here because of unit tests
                // Not all assemblies that referenced by the project implemented
                // for netcore3.1 project and not available during the tests run
            }
        }

        protected override void OnBindingContextChanged()
        {
            var vm = (ReceptionDetailsViewModel)BindingContext;
            if (vm != null)
            {
                vm.PropertyChanged -= Vm_PropertyChanged;
                if (vm.ReceptionGoods != null)
                {
                    vm.ReceptionGoods.CollectionChanged -= ReceptionGoods_CollectionChanged;
                }
            }
            base.OnBindingContextChanged();
            vm = (ReceptionDetailsViewModel)BindingContext;
            if (vm != null)
            {
                vm.PropertyChanged += Vm_PropertyChanged;
                vm.AnimateCounter += AnimateCounter;
            }
        }

        private void Vm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ReceptionDetailsViewModel.ReceptionGoods))
            {
                ((ReceptionDetailsViewModel)sender).ReceptionGoods.CollectionChanged += ReceptionGoods_CollectionChanged;
            }
        }

        /// <summary>
        /// ReceptionDetailsCollectionView can be null once is running in unit tests.
        /// </summary>
        private void ReceptionGoods_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if(e.Action == NotifyCollectionChangedAction.Add && ReceptionDetailsCollectionView != null)
            {
                ReceptionDetailsCollectionView.ScrollTo(0);
            }
        }

        async Task<bool> AnimateCounter()
        {
            // CountLabel can be null in unit tests
            if (CountLabel != null)
            {
                await CountLabel.ScaleTo(1.5, 300, Easing.BounceOut);
                await CountLabel.ScaleTo(1, 300, Easing.BounceOut);
            }
           
            return true;
        }
    }
}
