using WindowsClient.Shared.UIModels.MaterialBalance;

namespace WindowsClient.Features.CreateModel
{
    internal interface ICreateModelView
    {
        void ShowViewAsCreateDialog();
        void ShowViewAsEditDialog(Model model);
        void Close();
    }
}
