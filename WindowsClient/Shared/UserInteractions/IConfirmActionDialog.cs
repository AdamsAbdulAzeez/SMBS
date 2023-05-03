using System;

namespace WindowsClient.Shared.UserInteractions
{
    internal interface IConfirmActionDialog
    {
        void Confirm(string message, Action onConfirmed);
    }
}
