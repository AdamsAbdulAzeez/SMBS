﻿namespace WindowsClient.Services.Storage.FileStorage.XmlModels.MaterialBalance
{
    public class BoundedVariable
    {
        public string Name { get; set; }
        public double LowerBound { get; set; }
        public double UpperBound { get; set; }
        public double CurrentValue { get; set; }
    }
}
