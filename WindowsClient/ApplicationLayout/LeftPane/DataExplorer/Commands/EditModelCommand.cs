using System;
using AutoMapper;
using Prism.Commands;
using WindowsClient.Features.CreateModel;
using WindowsClient.Shared.UIModels.MaterialBalance;

namespace WindowsClient.ApplicationLayout.LeftPane.DataExplorer.Commands
{
    internal class EditModelCommand : DelegateCommand
    {
        public EditModelCommand(Func<ICreateModelView> getCreateModelView, IMapper mapper):base(() => {})
        {
            GetCreateModelView = getCreateModelView;
            Mapper = mapper;
        }

        protected override void Execute(object parameter)
        {
            var model = (Model)parameter;
            var clone = Mapper.Map(model, new Model());
            GetCreateModelView().ShowViewAsEditDialog(clone);
        }

        public Func<ICreateModelView> GetCreateModelView { get; }
        public IMapper Mapper { get; }
    }
}
