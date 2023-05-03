using System;
using System.Collections.Generic;

namespace WindowsClient.Services.Storage.FileStorage.XmlModels.MaterialBalance
{
    public class MaterialBalanceModel
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<Tank> Tanks { get; set; } = new();
    }
}
