using Warehouse.Mobile.Controls;
using Xamarin.Forms;
using Xunit;

namespace Warehouse.Mobile.UnitTests
{
    public class CustomEntryTests
    {
        [Fact]
        public void Creation()
        {
            Assert.NotNull(new CustomEntry());
        }

        [Fact]
        public void DigitsTypesToAllow()
        {
            var entry = new CustomEntry();
            entry.DigitsTypesToAllow = AllowedDigitTypes.Numbers;
            Assert.Equal(
                AllowedDigitTypes.Numbers,
                entry.DigitsTypesToAllow
            );
        }

        [Fact]
        public void NumberOfDecimalAccepted()
        {
            var entry = new CustomEntry();
            entry.NumberOfDecimalAccepted = 10;
            Assert.Equal(
                10,
                entry.NumberOfDecimalAccepted
            );
        }

        [Fact]
        public void MaxValue()
        {
            var entry = new CustomEntry();
            entry.MaxValue = 10;
            Assert.Equal(
                10,
                entry.MaxValue
            );
        }

        [Fact]
        public void EnterCommand()
        {
            var enterCommandCount = 0;
            var entry = new CustomEntry();
            entry.EnterCommand = new Command(() => enterCommandCount++);
            entry.EnterCommand.Execute(null);
            Assert.Equal(
                1,
                enterCommandCount
            );
        }

        [Fact]
        public void KeyPressedCommand()
        {
            var keyPressedCommandCount = 0;
            var entry = new CustomEntry();
            entry.KeyPressedCommand = new Command(() => keyPressedCommandCount++);
            entry.KeyPressedCommand.Execute(null);
            Assert.Equal(
                1,
                keyPressedCommandCount
            );
        }

        [Fact]
        public void Focused()
        {
            var entry = new CustomEntry();
            BindableFocus.SetIsFocused(entry, true);
            Assert.False(entry.IsFocused);
        }
    }
}
