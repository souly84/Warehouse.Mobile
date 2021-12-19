using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Prism.AppModel;
using Prism.Services;

namespace Warehouse.Mobile.Tests
{
    public class MockPageDialogService : IPageDialogService
    {
        public List<DialogPage> ShownDialogs { get; } = new List<DialogPage>();

        public Task<bool> DisplayAlertAsync(
            string title,
            string message,
            string acceptButton,
            string cancelButton)
        {
            ShownDialogs.Add(new DialogPage
            {
                Title = title,
                Message = message,
                AcceptButton = acceptButton,
                CancelButton = cancelButton,
            });
            return Task.FromResult(true);
        }

        public Task DisplayAlertAsync(string title, string message, string cancelButton)
        {
            ShownDialogs.Add(new DialogPage
            {
                Title = title,
                Message = message,
                CancelButton = cancelButton,
            });
            return Task.CompletedTask;
        }

        public Task<bool> DisplayAlertAsync(
            string title,
            string message,
            string acceptButton,
            string cancelButton,
            FlowDirection flowDirection)
        {
            ShownDialogs.Add(new DialogPage
            {
                Title = title,
                Message = message,
                AcceptButton = acceptButton,
                CancelButton = cancelButton,
            });
            return Task.FromResult(true);
        }

        public Task DisplayAlertAsync(string title, string message, string cancelButton, FlowDirection flowDirection)
        {
            ShownDialogs.Add(new DialogPage
            {
                Title = title,
                Message = message,
                CancelButton = cancelButton,
            });
            return Task.CompletedTask;
        }

        public Task<string> DisplayActionSheetAsync(
            string title,
            string cancelButton,
            string destroyButton,
            params string[] otherButtons)
        {
            ShownDialogs.Add(new DialogPage
            {
                Title = title,
                AcceptButton = destroyButton,
                CancelButton = cancelButton,
            });
            return Task.FromResult(string.Empty);
        }

        public Task DisplayActionSheetAsync(string title, params IActionSheetButton[] buttons)
        {
            ShownDialogs.Add(new DialogPage
            {
                Title = title,
            });
            return Task.CompletedTask;
        }

        public Task<string> DisplayActionSheetAsync(string title, string cancelButton, string destroyButton, FlowDirection flowDirection, params string[] otherButtons)
        {
            ShownDialogs.Add(new DialogPage
            {
                Title = title,
                AcceptButton = destroyButton,
                CancelButton = cancelButton,
            });
            return Task.FromResult(string.Empty);
        }

        public Task DisplayActionSheetAsync(string title, FlowDirection flowDirection, params IActionSheetButton[] buttons)
        {
            ShownDialogs.Add(new DialogPage
            {
                Title = title,
            });
            return Task.CompletedTask;
        }

        public Task<string> DisplayPromptAsync(string title, string message, string accept = "OK", string cancel = "Cancel", string placeholder = null, int maxLength = -1, KeyboardType keyboardType = KeyboardType.Default, string initialValue = "")
        {
            ShownDialogs.Add(new DialogPage
            {
                Title = title,
                Message = message,
                AcceptButton = accept,
                CancelButton = cancel,
            });
            return Task.FromResult(string.Empty);
        }

        public class DialogPage
        {
            public string Title { get; set; }

            public string Message { get; set; }

            public string AcceptButton { get; set; }

            public string CancelButton { get; set; }

            public override bool Equals(object obj)
            {
                return obj is DialogPage page &&
                       Title == page.Title &&
                       Message == page.Message &&
                       AcceptButton == page.AcceptButton &&
                       CancelButton == page.CancelButton;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Title, Message, AcceptButton, CancelButton);
            }
        }
    }
}
