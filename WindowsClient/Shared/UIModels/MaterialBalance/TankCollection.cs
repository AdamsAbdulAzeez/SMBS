using System;
using System.Collections.ObjectModel;

namespace WindowsClient.Shared.UIModels.MaterialBalance
{
    internal class TankCollection : ObservableCollection<Tank>
    {
        private readonly Guid _modelId;
        private TankCollection() { }
        public TankCollection(Guid modelId)
        {
            _modelId = modelId;
        }

        public new void Add(Tank tank)
        {
            tank.ModelId = _modelId;
            base.Add(tank);
        }
    }
}
