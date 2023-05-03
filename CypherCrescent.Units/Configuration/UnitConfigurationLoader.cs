using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using CypherCrescent.Units.Models;
using CypherCrescent.Units.XMLNodeModels;

namespace CypherCrescent.Units.Configuration
{
    internal class UnitSystemStore
    {
        public Task InitialiseInFlatFileModeAsync(string appDirectory)
        {
            return Task.Run(() =>
            {
                QuantityNodesCollection = CreateDefaultsOrReadSavedConfiguration<QuantityNode>("Quantities.xml", appDirectory)
                    .ToDictionary(node => node.Id, CreateQuantityFrom);

                UnitsNodeCollection = CreateDefaultsOrReadSavedConfiguration<UnitNode>("Units.xml", appDirectory)
                    .ToDictionary(node => node.Id, CreateUnitFromNode);

                MultiplierCollection = CreateDefaultsOrReadSavedConfiguration<MultiplierNode>("Multipliers.xml", appDirectory)
                    .ToDictionary(node => node.Id, CreateMultiplierFrom);

                EngineeringVariableCollection = CreateDefaultOrReadEngineeringVariables(appDirectory)
                    .ToDictionary(node => node.Name, node => node);

                CreateDefaultsOrReadSavedConfiguration<BaseUnitNode>("BaseUnits.xml", appDirectory).ToList()
                    .ForEach(baseUnit =>
                        QuantityNodesCollection[baseUnit.Quantity_ID].BaseUnit =
                            UnitsNodeCollection[baseUnit.BaseUnit_ID]);
            });
        }

        private Quantity CreateQuantityFrom(QuantityNode node)
        {
            var qty = Quantity.New(node.Name);
            qty.Id = node.Id;
            return qty;
        }

        private EngineeringVariable CreateEngineeringVariableFrom(EngineeringVariableNode node)
        {
            Unit databaseUnit = null;
            Unit defaultUnit = null;
            Multiplier databaseMultiplier = null;
            Multiplier defaultMultiplier = null;

            if (node.DefaultUnit_Id > 0)
                defaultUnit = UnitsNodeCollection[node.DefaultUnit_Id];

            if (node.DatabaseUnit_Id > 0)
                databaseUnit = UnitsNodeCollection[node.DatabaseUnit_Id];

            if (node.DefaultUnitMultiplier_Id > 0)
                defaultMultiplier = MultiplierCollection[node.DefaultUnitMultiplier_Id];

            if (node.DatabaseUnitMultiplier_Id > 0)
                databaseMultiplier = MultiplierCollection[node.DatabaseUnitMultiplier_Id];

            var variable = EngineeringVariable.New(node.Name, databaseUnit, defaultUnit, databaseMultiplier, defaultMultiplier);
            variable.Id = node.Id;

            return variable;
        }

        private Multiplier CreateMultiplierFrom(MultiplierNode node)
        {
            var multiplier = Multiplier.New(
                node.MultiplierSymbol,
                node.Multiplicand,
                node.MultiplierType,
                UnitsNodeCollection[node.UnitId]
            );

            multiplier.Id = node.Id;
            return multiplier;
        }

        private Unit CreateUnitFromNode(UnitNode node)
        {
            var unit = Unit.New(
                node.Symbol,
                node.FullName,
                node.Type,
                node.Multiplicand,
                node.Addend,
                QuantityNodesCollection[node.Quantity_Id]
            );
            unit.Id = node.Id;
            return unit;
        }


        public EngineeringVariable GetVariable(string variableName)
        {
            return EngineeringVariableCollection[variableName];
        }

        public IList<EngineeringVariable> CreateDefaultOrReadEngineeringVariables(string appDirectory)
        {
            var appPath = Path.Combine(appDirectory, "Variables.xml");
            if (File.Exists(appPath))
            {
                var serializer = new XmlSerializer(typeof(List<EngineeringVariable>), new XmlRootAttribute("dataroot"));
                var xmlString = File.ReadAllText(appPath);
                var stringReader = new StringReader(xmlString);
                var deserializedOutput = (List<EngineeringVariable>)serializer.Deserialize(stringReader);
                return deserializedOutput;
            }
            else
            {
                var executableLocation = Path.GetDirectoryName(typeof(UnitSystemStore).Assembly.Location);
                var defaultFilePath = Path.Combine(executableLocation, "Variables.csv");
                var lines = File.ReadAllLines(defaultFilePath);
                return lines.Select(line => line.Split(','))
                    .Skip(1) //header
                    .Select(line => EngineeringVariable.New(
                        line[0],
                        UnitsNodeCollection[int.Parse(line[1])],
                        UnitsNodeCollection[int.Parse(line[2])],
                        line[3] == "0" ? null : MultiplierCollection[int.Parse(line[3])],
                        line[4] == "0" ? null : MultiplierCollection[int.Parse(line[4])]
                    ))
                    .ToList();
            }

        }

        /// <summary>
        /// Attempts to read saved unit system configurations from application directory. If files do not exist, they will be created.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlFileName"></param>
        /// <returns></returns>
        public static IList<T> CreateDefaultsOrReadSavedConfiguration<T>(string xmlFileName, string appDirectory)
        {
            var executableLocation = Path.GetDirectoryName(typeof(UnitSystemStore).Assembly.Location);
            var defaultFilePath = Path.Combine(executableLocation, xmlFileName);
            var appPath = Path.Combine(appDirectory, xmlFileName);

            if (!Directory.Exists(appDirectory))
                Directory.CreateDirectory(appDirectory);

            if (File.Exists(appPath))
            {
                var serializer = new XmlSerializer(typeof(List<T>), new XmlRootAttribute("dataroot"));
                var xmlString = File.ReadAllText(appPath);
                var stringReader = new StringReader(xmlString);
                var deserializedOutput = (List<T>)serializer.Deserialize(stringReader);
                return deserializedOutput;
            }
            else
            {
                var serializer = new XmlSerializer(typeof(List<T>), new XmlRootAttribute("dataroot"));
                var xmlString = File.ReadAllText(defaultFilePath);
                File.WriteAllText(appPath, xmlString);
                var stringReader = new StringReader(xmlString);
                var deserializedOutput = (List<T>)serializer.Deserialize(stringReader);
                return deserializedOutput;
            }
        }

        public Dictionary<int, Quantity> QuantityNodesCollection { get; private set; }
        public Dictionary<int, Unit> UnitsNodeCollection { get; private set; }
        public Dictionary<int, Multiplier> MultiplierCollection { get; private set; }
        public Dictionary<string, EngineeringVariable> EngineeringVariableCollection { get; private set; }

    }
}
