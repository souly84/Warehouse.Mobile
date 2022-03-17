using System;
namespace Warehouse.Mobile.Interfaces
{
    public interface IEnvironment
    {
        string GetDeviceId();

        string GetBuildVersion();

        void ExitApp();

        string GetApplicationPath();

        void HideKeyboard();

        void BeepLoudly();

        void SaveFile(string fileName, string text);

        string LoadFile(string fileName);

        string GetCurrentFirstIPAddress();
    }

    public class MockEnvironment : IEnvironment
    {
        private readonly string _buildVersion;

        public MockEnvironment()
            : this("1.0.0")
        {
        }

        public MockEnvironment(string buildVersion)
        {
            _buildVersion = buildVersion;
        }

        public bool AppClosed { get; private set; }

        public string GetBuildVersion()
        {
            return _buildVersion;
        }

        public void ExitApp()
        {
            AppClosed = true;
        }

        public void BeepLoudly()
        {
            return;
        }

        public string GetApplicationPath()
        {
            return "aplicationPath";
        }

        public string GetCurrentFirstIPAddress()
        {
            return "127.0.0.1";
        }

        public string GetDeviceId()
        {
            return "123456789";
        }

        public void HideKeyboard()
        {
            return;
        }

        public string LoadFile(string fileName)
        {
            return fileName;
        }

        public void SaveFile(string fileName, string text)
        {
            return;
        }
    }
}
