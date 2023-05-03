using System;

namespace WindowsClient.Shared.UIModels.MaterialBalance
{
    internal class Model
    {
        internal Model() 
        {
            Id = Guid.NewGuid();
            Tanks = new TankCollection(Id);
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public TankCollection Tanks { get; set; }
    }
}
