using Warehouse.Mobile.Controls;
using Xamarin.Forms;
using Xunit;

namespace Warehouse.Mobile.UnitTests
{
    public class MaterialSearchBarTests
    {
        [Fact]
        public void Creation()
        {
            Assert.NotNull(new MaterialSearchBar());
        }

        [Fact]
        public void BackgroundColor()
        {
            var bar = new MaterialSearchBar();
            bar.BackgroundColor = Color.Red;
            Assert.Equal(
                Color.Red,
                bar.BackgroundColor
            );
        }

        [Fact]
        public void CornerRadius()
        {
            var bar = new MaterialSearchBar();
            bar.CornerRadius = 10;
            Assert.Equal(
                10,
                bar.CornerRadius
            );
        }

        [Fact]
        public void Padding()
        {
            var bar = new MaterialSearchBar();
            bar.Padding = 10;
            Assert.Equal(
                10,
                bar.Padding
            );
        }
    }
}
