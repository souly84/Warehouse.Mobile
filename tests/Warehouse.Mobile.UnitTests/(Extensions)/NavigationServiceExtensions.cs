using System.Reflection;
using Prism.Navigation;

namespace Warehouse.Mobile.UnitTests
{
    public static class NavigationServiceExtensions
    {
        public static void ResetPageNavigationRegistry()
        {
            TypeInfo staticType = typeof(PageNavigationRegistry).GetTypeInfo();
            ConstructorInfo ci = null;
            foreach (var ctor in staticType.DeclaredConstructors)
            {
                ci = ctor;
                continue;
            }

            object[] parameters = new object[0];
            ci.Invoke(null, parameters);
        }
    }
}
