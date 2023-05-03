using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CypherCrescent.Units.Variables;
using WindowsClient.Shared.ErrorHandling;
using WindowsClient.Shared.UIModels.MaterialBalance;
using CypherCrescent.Units.Services;

namespace WindowsClient.Services.ExcelImport
{
    internal sealed class MaterialBalanceDataReader : IMaterialBalanceDataReader, IDisposable
    {
        private IWorkbook book;
        private string selectedSheet;
        private readonly IUnitService _unitService;
        public MaterialBalanceDataReader(IUnitService unitService) => _unitService = unitService;

        public Task<IActionResult<IList<PvtDataRow>>> ReadExternalPvtData(FluidType fluidType)
        {
            return fluidType switch
            {
                FluidType.Oil => ReadOilExternalPvtData(),
                FluidType.Gas => ReadGasExternalPvtData(),
                FluidType.Condensate => ReadCondensateExternalPvtData(),
                _ => Task.FromResult(new FailedResult<IList<PvtDataRow>>(
                                    new List<string> { "Failed to read external PVT data" })
                                    as IActionResult<IList<PvtDataRow>>),
            };
        }

        private Task<IActionResult<IList<PvtDataRow>>> ReadCondensateExternalPvtData()
        {
            try
            {
                var setupResult = SetupWorkbook();
                if (setupResult.IsFailure) throw new ArgumentException();

                ISheet externalPvtDataSheet = book.GetSheet(selectedSheet);
                int rowShift = 0, dataRowsCount = externalPvtDataSheet.LastRowNum - rowShift;
                IRow row = externalPvtDataSheet.GetRow(rowShift);
                var externalPvtData = new List<PvtDataRow>();

                for (int i = 1; i <= dataRowsCount; i++)
                {
                    row = externalPvtDataSheet.GetRow(rowShift + i);
                    PvtDataRow data = new PvtDataRow
                    {
                        Temperature = row.GetCell(0).NumericCellValue,
                        Pressure = row.GetCell(1).NumericCellValue,
                        DewPoint = row.GetCell(2).NumericCellValue,
                        ReservoirCGR = row.GetCell(3).NumericCellValue,
                        ZFactor = row.GetCell(4).NumericCellValue,
                        GasFVF = row.GetCell(5).NumericCellValue,
                        GasViscosity = row.GetCell(6).NumericCellValue,
                        GasDensity = row.GetCell(7).NumericCellValue,
                        PseudoPressure = row.GetCell(8).NumericCellValue,
                        OilFVF = row.GetCell(9).NumericCellValue,
                        OilViscosity = row.GetCell(10).NumericCellValue,
                        OilDensity = row.GetCell(11).NumericCellValue,
                        WaterFVF = row.GetCell(12).NumericCellValue,
                        WaterViscosity = row.GetCell(13).NumericCellValue,
                        WaterDensity = row.GetCell(14).NumericCellValue,
                        WaterCompressibility = row.GetCell(15).NumericCellValue,
                        VaporizedCGR = row.GetCell(16).NumericCellValue,
                        VapourisedWGR = row.GetCell(17).NumericCellValue,
                    };

                    externalPvtData.Add(data);
                }
                return Task.FromResult(new SuccessfulAction<IList<PvtDataRow>>(externalPvtData)
                    as IActionResult<IList<PvtDataRow>>);
            }
            catch (Exception e)
            {
                return Task.FromResult(new FailedResult<IList<PvtDataRow>>(
                    new List<string> { "Failed to read external PVT data", e?.Message })
                    as IActionResult<IList<PvtDataRow>>);
            }
        }

        private Task<IActionResult<IList<PvtDataRow>>> ReadGasExternalPvtData()
        {
            try
            {
                var setupResult = SetupWorkbook();
                if (setupResult.IsFailure) throw new ArgumentException();

                ISheet externalPvtDataSheet = book.GetSheet(selectedSheet);
                int rowShift = 0, dataRowsCount = externalPvtDataSheet.LastRowNum - rowShift;
                IRow row = externalPvtDataSheet.GetRow(rowShift);
                var externalPvtData = new List<PvtDataRow>();

                for (int i = 1; i <= dataRowsCount; i++)
                {
                    row = externalPvtDataSheet.GetRow(rowShift + i);
                    PvtDataRow data = new PvtDataRow
                    {
                        Temperature = row.GetCell(0).NumericCellValue,
                        Pressure = row.GetCell(1).NumericCellValue,
                        ZFactor = row.GetCell(2).NumericCellValue,
                        GasFVF = row.GetCell(3).NumericCellValue,
                        GasViscosity = row.GetCell(4).NumericCellValue,
                        GasDensity = row.GetCell(5).NumericCellValue,
                        PseudoPressure = row.GetCell(6).NumericCellValue,
                        WaterFVF = row.GetCell(7).NumericCellValue,
                        WaterViscosity = row.GetCell(8).NumericCellValue,
                        WaterDensity = row.GetCell(9).NumericCellValue,
                        WaterCompressibility = row.GetCell(10).NumericCellValue,
                    };

                    externalPvtData.Add(data);
                }
                return Task.FromResult(new SuccessfulAction<IList<PvtDataRow>>(externalPvtData)
                    as IActionResult<IList<PvtDataRow>>);
            }
            catch (Exception e)
            {
                return Task.FromResult(new FailedResult<IList<PvtDataRow>>(
                    new List<string> { "Failed to read external PVT data", e?.Message })
                    as IActionResult<IList<PvtDataRow>>);
            }
        }

        private Task<IActionResult<IList<PvtDataRow>>> ReadOilExternalPvtData()
        {
            try
            {
                var setupResult = SetupWorkbook();
                if (setupResult.IsFailure) throw new ArgumentException();

                ISheet externalPvtDataSheet = book.GetSheet(selectedSheet);
                int rowShift = 0, dataRowsCount = externalPvtDataSheet.LastRowNum - rowShift;
                IRow row = externalPvtDataSheet.GetRow(rowShift);
                var externalPvtData = new List<PvtDataRow>();

                for (int i = 1; i <= dataRowsCount; i++)
                {
                    if (i == 102) break;
                    row = externalPvtDataSheet.GetRow(rowShift + i);
                    PvtDataRow data = new PvtDataRow
                    {
                        Temperature = row.GetCell(0).NumericCellValue,
                        Pressure = row.GetCell(1).NumericCellValue,
                        BubblePoint = row.GetCell(2).NumericCellValue,
                        GasOilRatio = row.GetCell(3).NumericCellValue,
                        OilFVF = row.GetCell(4).NumericCellValue,
                        OilViscosity = row.GetCell(5).NumericCellValue,
                        ZFactor = row.GetCell(6).NumericCellValue,
                        GasFVF = row.GetCell(7).NumericCellValue,
                        GasViscosity = row.GetCell(8).NumericCellValue,
                        OilDensity = row.GetCell(9).NumericCellValue,
                        GasDensity = row.GetCell(10).NumericCellValue,
                        WaterFVF = row.GetCell(11).NumericCellValue,
                        WaterViscosity = row.GetCell(12).NumericCellValue,
                        WaterDensity = row.GetCell(13).NumericCellValue,
                        WaterCompressibility = row.GetCell(14).NumericCellValue,
                    };

                    externalPvtData.Add(data);
                }
                return Task.FromResult(new SuccessfulAction<IList<PvtDataRow>>(externalPvtData)
                    as IActionResult<IList<PvtDataRow>>);
            }
            catch (Exception e)
            {
                return Task.FromResult(new FailedResult<IList<PvtDataRow>>(
                    new List<string> { "Failed to read external PVT data", e?.Message })
                    as IActionResult<IList<PvtDataRow>>);
            }
        }

        public Task<IActionResult<IList<ProductionDataRow>>> ReadProductionData()
        {
            try
            {
                var setupResult = SetupWorkbook();

                if (setupResult.IsFailure) throw new ArgumentException();

                ISheet productionDataSheet = book.GetSheet(selectedSheet);
                int rowShift = 0, dataRowsCount = productionDataSheet.LastRowNum - rowShift;
                IRow row = productionDataSheet.GetRow(rowShift);

                var productionData = new List<ProductionDataRow>();
                for (int i = 1; i <= dataRowsCount; i++)
                {
                    row = productionDataSheet.GetRow(rowShift + i);

                    if (row.GetCell(6).DateCellValue == default) break;

                    productionData.Add(new ProductionDataRow
                    {
                        Time = row.GetCell(0).DateCellValue,
                        Pressure = row.GetCell(1).NumericCellValue,
                        OilProduced = row.GetCell(2).NumericCellValue,
                        GasProduced = row.GetCell(3).NumericCellValue,
                        WaterProduced = row.GetCell(4).NumericCellValue,
                        GasInjected = row.GetCell(5).NumericCellValue,
                        WaterInjected = row.GetCell(6).NumericCellValue
                    });
                }
                return Task.FromResult(new SuccessfulAction<IList<ProductionDataRow>>(productionData)
                as IActionResult<IList<ProductionDataRow>>);
            }
            catch (ArgumentException)
            {
                return Task.FromResult(new FailedResult<IList<ProductionDataRow>>() as IActionResult<IList<ProductionDataRow>>);
            }
            catch (Exception e)
            {
                return Task.FromResult(new FailedResult<IList<ProductionDataRow>>(
                    new List<string> { "Failed to read production data", e?.Message })
                        as IActionResult<IList<ProductionDataRow>>);
            }
        }

        public Task<IActionResult<Tank>> ReadPvtMatchingInputData(FluidType fluidType)
        {
            return fluidType switch
            {
                FluidType.Oil => ReadOilPvtMatchingInputData(),
                FluidType.Gas => ReadGasPvtMatchingInputData(),
                FluidType.Condensate => ReadCondensatePvtMatchingInputData(),
                _ => Task.FromResult(new FailedResult<Tank>(
                                    new List<string> { "Failed to read PVT input data" })
                                    as IActionResult<Tank>),
            };
        }

        private Task<IActionResult<Tank>> ReadCondensatePvtMatchingInputData()
        {
            try
            {
                var setupResult = SetupWorkbook();

                if (setupResult.IsFailure) throw new ArgumentException();

                var tank = new Tank();
                var pvtInitialCondition = ReadCondensatePvtInitialCondition().Result.Payload;
                var pvtLabData = ReadCondensateLabPvtData().Result.Payload;

                tank.PvtInitialCondition = pvtInitialCondition;
                tank.SetPvtMatchingInput(pvtLabData.ToList());

                return Task.FromResult(new SuccessfulAction<Tank>(tank)
                    as IActionResult<Tank>);
            }
            catch (ArgumentException)
            {
                return Task.FromResult(new FailedResult<Tank>() as IActionResult<Tank>);
            }
            catch (Exception e)
            {
                return Task.FromResult(new FailedResult<Tank>(
                    new List<string> { "Failed to read PVT data", e.Message })
                        as IActionResult<Tank>);
            }
        }

        private Task<IActionResult<Tank>> ReadGasPvtMatchingInputData()
        {
            try
            {
                var setupResult = SetupWorkbook();

                if (setupResult.IsFailure) throw new ArgumentException();

                var tank = new Tank();
                var pvtInitialCondition = ReadGasPvtInitialCondition().Result.Payload;
                var pvtLabData = ReadGasLabPvtData().Result.Payload;

                tank.PvtInitialCondition = pvtInitialCondition;
                tank.SetPvtMatchingInput(pvtLabData.ToList());

                return Task.FromResult(new SuccessfulAction<Tank>(tank)
                    as IActionResult<Tank>);
            }
            catch (ArgumentException)
            {
                return Task.FromResult(new FailedResult<Tank>() as IActionResult<Tank>);
            }
            catch (Exception e)
            {
                return Task.FromResult(new FailedResult<Tank>(
                    new List<string> { "Failed to read PVT data", e.Message })
                        as IActionResult<Tank>);
            }
        }

        private Task<IActionResult<Tank>> ReadOilPvtMatchingInputData()
        {
            try
            {
                var setupResult = SetupWorkbook();

                if (setupResult.IsFailure) throw new ArgumentException();

                var tank = new Tank();
                var pvtInitialCondition = ReadOilPvtInitialCondition().Result.Payload;
                var pvtLabData = ReadOilLabPvtData().Result.Payload;

                tank.PvtInitialCondition = pvtInitialCondition;
                tank.BubblePointPressure = pvtInitialCondition.BubblePoint;
                tank.SetPvtMatchingInput(pvtLabData.ToList());

                return Task.FromResult(new SuccessfulAction<Tank>(tank)
                    as IActionResult<Tank>);
            }
            catch (ArgumentException)
            {
                return Task.FromResult(new FailedResult<Tank>() as IActionResult<Tank>);
            }
            catch (Exception e)
            {
                return Task.FromResult(new FailedResult<Tank>(
                    new List<string> { "Failed to read PVT data", e.Message })
                        as IActionResult<Tank>);
            }
        }

        public Task<IActionResult<Tank>> ReadHistoryMatchingInputData()
        {
            try
            {
                var setupResult = SetupWorkbook();

                if (setupResult.IsFailure) throw new ArgumentException();

                var tank = ReadTankData().Result.Payload;
                var aquifer = ReadAquiferData().Result.Payload;
                var relativePermeabilityData = ReadRelativePermeabilityData().Result.Payload;
                var rock = ReadRockData().Result.Payload;

                tank.RelPermData = relativePermeabilityData;
                tank.Rock = rock;
                tank.SetTankAquifer(aquifer);

                return Task.FromResult(new SuccessfulAction<Tank>(tank)
                    as IActionResult<Tank>);
            }
            catch (ArgumentException)
            {
                return Task.FromResult(new FailedResult<Tank>() as IActionResult<Tank>);
            }
            catch (Exception e)
            {
                return Task.FromResult(new FailedResult<Tank>(
                    new List<string> { "Failed to read tank data", e.Message })
                        as IActionResult<Tank>);
            }

        }

        private Task<IActionResult<Aquifer>> ReadAquiferData()
        {
            try
            {
                ISheet aquiferDataSheet = book.GetSheet(selectedSheet);
                int rowShift = 15;
                IRow row = aquiferDataSheet.GetRow(rowShift);
                var position = ConvertToPosition(row.GetCell(1).StringCellValue);

                row = aquiferDataSheet.GetRow(rowShift + 1);
                var geometry = ConvertToGeometry(row.GetCell(1).StringCellValue);

                row = aquiferDataSheet.GetRow(rowShift + 2);
                var boundaryType = ConvertToBoundary(row.GetCell(1).StringCellValue);

                row = aquiferDataSheet.GetRow(rowShift + 3);
                var modelType = ConvertToWaterInfluxModel(row.GetCell(1).StringCellValue);

                rowShift++;
                row = aquiferDataSheet.GetRow(rowShift + 4);

                var outerInnerRadiusRatio = new BoundedVariable<OuterInnerRadiusRatio>
                {
                    LowerBound = row.GetCell(1).NumericCellValue,
                    UpperBound = row.GetCell(2).NumericCellValue
                };
                outerInnerRadiusRatio.CurrentValue = (outerInnerRadiusRatio.LowerBound + outerInnerRadiusRatio.UpperBound) / 2;

                _unitService.RegisterVariable(outerInnerRadiusRatio.LowerBound);
                _unitService.RegisterVariable(outerInnerRadiusRatio.UpperBound);
                _unitService.RegisterVariable(outerInnerRadiusRatio.CurrentValue);

                row = aquiferDataSheet.GetRow(rowShift + 5);
                var encroachmentAngle = new BoundedVariable<EncroachmentAngle>
                {
                    LowerBound = row.GetCell(1).NumericCellValue,
                    UpperBound = row.GetCell(2).NumericCellValue
                };
                encroachmentAngle.CurrentValue = (encroachmentAngle.LowerBound + encroachmentAngle.UpperBound) / 2;

                _unitService.RegisterVariable(encroachmentAngle.UpperBound);
                _unitService.RegisterVariable(encroachmentAngle.LowerBound);
                _unitService.RegisterVariable(encroachmentAngle.CurrentValue);

                row = aquiferDataSheet.GetRow(rowShift + 6);
                var volume = new BoundedVariable<Volume>
                {
                    LowerBound = row.GetCell(1).NumericCellValue,
                    UpperBound = row.GetCell(2).NumericCellValue
                };
                volume.CurrentValue = (volume.LowerBound + volume.UpperBound) / 2;

                _unitService.RegisterVariable(volume.LowerBound);
                _unitService.RegisterVariable(volume.UpperBound);
                _unitService.RegisterVariable(volume.CurrentValue);

                row = aquiferDataSheet.GetRow(rowShift + 7);
                var anisotropy = new BoundedVariable<Dimensionless>
                {
                    LowerBound = row.GetCell(1).NumericCellValue,
                    UpperBound = row.GetCell(2).NumericCellValue
                };
                anisotropy.CurrentValue = (anisotropy.LowerBound + anisotropy.UpperBound) / 2;

                _unitService.RegisterVariable(anisotropy.LowerBound);
                _unitService.RegisterVariable(anisotropy.UpperBound);
                _unitService.RegisterVariable(anisotropy.CurrentValue);

                var aquifer = new Aquifer
                {
                    Configuration = new AquiferConfiguration { Position = position, Geometry = geometry},
                    //AquiferModelType = new AquiferModel { ModelType = modelType },
                    BoundaryType = boundaryType,
                    ModelType = modelType,
                    OuterInnerRadiusRatio = outerInnerRadiusRatio,
                    EncroachmentAngle = encroachmentAngle,
                    Volume = volume,
                    Anisotropy = anisotropy
                    
                };
                return Task.FromResult(new SuccessfulAction<Aquifer>(aquifer)
                    as IActionResult<Aquifer>);
            }
            catch (Exception e)
            {
                return Task.FromResult(new FailedResult<Aquifer>(
                    new List<string> { "Failed to read aquifer data", e?.Message })
                        as IActionResult<Aquifer>);
            }
        }

        private Task<IActionResult<IList<PvtDataRow>>> ReadCondensateLabPvtData()
        {
            try
            {
                ISheet labPvtDataSheet = book.GetSheet(selectedSheet);
                int rowShift = 3, dataRowsCount = labPvtDataSheet.LastRowNum - rowShift;
                IRow row = labPvtDataSheet.GetRow(rowShift);

                var labPvtData = new List<PvtDataRow>();

                for (int i = 1; i <= dataRowsCount; i++)
                {
                    row = labPvtDataSheet.GetRow(rowShift + i);
                    PvtDataRow data = new PvtDataRow
                    {
                        Pressure = row.GetCell(0).NumericCellValue,
                        ReservoirCGR = row.GetCell(1).NumericCellValue,
                        VaporizedCGR = row.GetCell(2).NumericCellValue,
                        ZFactor = row.GetCell(3).NumericCellValue,
                        GasFVF = row.GetCell(4).NumericCellValue,
                        GasViscosity = row.GetCell(5).NumericCellValue,
                        OilFVF = row.GetCell(5).NumericCellValue,
                    };

                    labPvtData.Add(data);
                }
                return Task.FromResult(new SuccessfulAction<IList<PvtDataRow>>(labPvtData)
                    as IActionResult<IList<PvtDataRow>>);
            }
            catch (Exception e)
            {
                return Task.FromResult(new FailedResult<IList<PvtDataRow>>(
                    new List<string> { "Failed to read PVT matching input data", e?.Message })
                    as IActionResult<IList<PvtDataRow>>);
            }
        }

        private Task<IActionResult<IList<PvtDataRow>>> ReadGasLabPvtData()
        {
            try
            {
                ISheet labPvtDataSheet = book.GetSheet(selectedSheet);
                int rowShift = 3, dataRowsCount = labPvtDataSheet.LastRowNum - rowShift;
                IRow row = labPvtDataSheet.GetRow(rowShift);

                var labPvtData = new List<PvtDataRow>();

                for (int i = 1; i <= dataRowsCount; i++)
                {
                    row = labPvtDataSheet.GetRow(rowShift + i);
                    PvtDataRow data = new PvtDataRow
                    {
                        Pressure = row.GetCell(0).NumericCellValue,
                        ZFactor = row.GetCell(1).NumericCellValue,
                        GasFVF = row.GetCell(2).NumericCellValue,
                        GasViscosity = row.GetCell(3).NumericCellValue,
                    };

                    labPvtData.Add(data);
                }
                return Task.FromResult(new SuccessfulAction<IList<PvtDataRow>>(labPvtData)
                    as IActionResult<IList<PvtDataRow>>);
            }
            catch (Exception e)
            {
                return Task.FromResult(new FailedResult<IList<PvtDataRow>>(
                    new List<string> { "Failed to read PVT matching input data", e?.Message })
                    as IActionResult<IList<PvtDataRow>>);
            }
        }

        private Task<IActionResult<IList<PvtDataRow>>> ReadOilLabPvtData()
        {
            try
            {
                ISheet labPvtDataSheet = book.GetSheet(selectedSheet);
                int rowShift = 3, dataRowsCount = labPvtDataSheet.LastRowNum - rowShift;
                IRow row = labPvtDataSheet.GetRow(rowShift);

                var labPvtData = new List<PvtDataRow>();

                for (int i = 1; i <= dataRowsCount; i++)
                {
                    row = labPvtDataSheet.GetRow(rowShift + i);
                    PvtDataRow data = new()
                    {
                        Pressure = row.GetCell(0).NumericCellValue,
                        GasOilRatio = row.GetCell(1).NumericCellValue,
                        OilFVF = row.GetCell(2).NumericCellValue,
                        OilViscosity = row.GetCell(3).NumericCellValue,
                        GasFVF = row.GetCell(4).NumericCellValue,
                        GasViscosity = row.GetCell(5).NumericCellValue,
                    };

                    labPvtData.Add(data);
                }
                return Task.FromResult(new SuccessfulAction<IList<PvtDataRow>>(labPvtData)
                    as IActionResult<IList<PvtDataRow>>);
            }
            catch (Exception e)
            {
                return Task.FromResult(new FailedResult<IList<PvtDataRow>>(
                    new List<string> { "Failed to read PVT matching input data", e?.Message })
                    as IActionResult<IList<PvtDataRow>>);
            }
        }

        private Task<IActionResult<PvtInitialCondition>> ReadCondensatePvtInitialCondition()
        {
            try
            {
                ISheet pvtInitialConditionDataSheet = book.GetSheet(selectedSheet);
                int rowShift = 1;
                IRow row = pvtInitialConditionDataSheet.GetRow(rowShift);
                //TODO: Read correct properties
                var pvtInitialConditions = new PvtInitialCondition
                {
                    GOR = row.GetCell(0).NumericCellValue,
                    OilGravity = row.GetCell(1).NumericCellValue,
                    GasGravity = row.GetCell(2).NumericCellValue,
                    WaterSalinity = row.GetCell(3).NumericCellValue,
                    MoleH2S = row.GetCell(4).NumericCellValue,
                    MoleCO2 = row.GetCell(5).NumericCellValue,
                    MoleN2 = row.GetCell(6).NumericCellValue,
                    Temperature = row.GetCell(7).NumericCellValue,
                    BubblePoint = row.GetCell(8).NumericCellValue,
                };

                return Task.FromResult(new SuccessfulAction<PvtInitialCondition>(pvtInitialConditions)
                    as IActionResult<PvtInitialCondition>);
            }
            catch (Exception e)
            {
                return Task.FromResult(new FailedResult<PvtInitialCondition>(
                    new List<string> { "Failed to read PVT initial conditions", e?.Message })
                        as IActionResult<PvtInitialCondition>);
            }
        }

        private Task<IActionResult<PvtInitialCondition>> ReadGasPvtInitialCondition()
        {
            try
            {
                ISheet pvtInitialConditionDataSheet = book.GetSheet(selectedSheet);
                int rowShift = 1;
                IRow row = pvtInitialConditionDataSheet.GetRow(rowShift);
                //TODO: Read correct properties
                var pvtInitialConditions = new PvtInitialCondition
                {
                    GOR = row.GetCell(0).NumericCellValue,
                    OilGravity = row.GetCell(1).NumericCellValue,
                    GasGravity = row.GetCell(2).NumericCellValue,
                    WaterSalinity = row.GetCell(3).NumericCellValue,
                    MoleH2S = row.GetCell(4).NumericCellValue,
                    MoleCO2 = row.GetCell(5).NumericCellValue,
                    MoleN2 = row.GetCell(6).NumericCellValue,
                    Temperature = row.GetCell(7).NumericCellValue,
                    BubblePoint = row.GetCell(8).NumericCellValue,
                };

                return Task.FromResult(new SuccessfulAction<PvtInitialCondition>(pvtInitialConditions)
                    as IActionResult<PvtInitialCondition>);
            }
            catch (Exception e)
            {
                return Task.FromResult(new FailedResult<PvtInitialCondition>(
                    new List<string> { "Failed to read PVT initial conditions", e?.Message })
                        as IActionResult<PvtInitialCondition>);
            }
        }

        private Task<IActionResult<PvtInitialCondition>> ReadOilPvtInitialCondition()
        {
            try
            {
                ISheet pvtInitialConditionDataSheet = book.GetSheet(selectedSheet);
                int rowShift = 1;
                IRow row = pvtInitialConditionDataSheet.GetRow(rowShift);

                var pvtInitialConditions = new PvtInitialCondition
                {
                    GOR = row.GetCell(0).NumericCellValue,
                    OilGravity = row.GetCell(1).NumericCellValue,
                    GasGravity = row.GetCell(2).NumericCellValue,
                    WaterSalinity = row.GetCell(3).NumericCellValue,
                    MoleH2S = row.GetCell(4).NumericCellValue,
                    MoleCO2 = row.GetCell(5).NumericCellValue,
                    MoleN2 = row.GetCell(6).NumericCellValue,
                    Temperature = row.GetCell(7).NumericCellValue,
                    BubblePoint = row.GetCell(8).NumericCellValue,
                };

                return Task.FromResult(new SuccessfulAction<PvtInitialCondition>(pvtInitialConditions)
                    as IActionResult<PvtInitialCondition>);
            }
            catch (Exception e)
            {
                return Task.FromResult(new FailedResult<PvtInitialCondition>(
                    new List<string> { "Failed to read PVT initial conditions", e?.Message })
                        as IActionResult<PvtInitialCondition>);
            }
        }

        private Task<IActionResult<RelativePermeabilityData>> ReadRelativePermeabilityData()
        {
            try
            {
                ISheet tankDataSheet = book.GetSheet(selectedSheet);
                int rowShift = 32;
                IRow row = tankDataSheet.GetRow(rowShift);

                var waterRelPerm = new RelativePermeabilityDataRow
                {
                    ResidualSaturation = row.GetCell(1).NumericCellValue,
                    EndPoint = row.GetCell(2).NumericCellValue,
                    Exponent = row.GetCell(3).NumericCellValue
                };

                row = tankDataSheet.GetRow(rowShift + 1);
                var oilRelPerm = new RelativePermeabilityDataRow
                {
                    ResidualSaturation = row.GetCell(1).NumericCellValue,
                    EndPoint = row.GetCell(2).NumericCellValue,
                    Exponent = row.GetCell(3).NumericCellValue
                };

                row = tankDataSheet.GetRow(rowShift + 2);
                var gasRelPerm = new RelativePermeabilityDataRow
                {
                    ResidualSaturation = row.GetCell(1).NumericCellValue,
                    EndPoint = row.GetCell(2).NumericCellValue,
                    Exponent = row.GetCell(3).NumericCellValue
                };

                var relPermData = new RelativePermeabilityData
                {
                    WaterRelPerm = waterRelPerm,
                    OilRelPerm = oilRelPerm,
                    GasRelPerm = gasRelPerm
                };
                return Task.FromResult(new SuccessfulAction<RelativePermeabilityData>(relPermData)
                    as IActionResult<RelativePermeabilityData>);
            }
            catch (Exception e)
            {
                return Task.FromResult(new FailedResult<RelativePermeabilityData>(
                    new List<string> { "Failed to read relative permeability data", e?.Message })
                        as IActionResult<RelativePermeabilityData>);
            }
        }

        private Task<IActionResult<Rock>> ReadRockData()
        {
            try
            {
                ISheet aquiferDataSheet = book.GetSheet(selectedSheet);
                int rowShift = 27;
                IRow row = aquiferDataSheet.GetRow(rowShift);

                var permeability = new BoundedVariable<ReservoirPermeability>
                {
                    LowerBound = row.GetCell(1).NumericCellValue,
                    UpperBound = row.GetCell(2).NumericCellValue
                };
                permeability.CurrentValue = (permeability.LowerBound + permeability.UpperBound) / 2;

                _unitService.RegisterVariable(permeability.LowerBound);
                _unitService.RegisterVariable(permeability.UpperBound);
                _unitService.RegisterVariable(permeability.CurrentValue);

                row = aquiferDataSheet.GetRow(rowShift + 1);
                var porosity = new BoundedVariable<Porosity>
                {
                    LowerBound = row.GetCell(1).NumericCellValue,
                    UpperBound = row.GetCell(2).NumericCellValue
                };
                porosity.CurrentValue = (porosity.LowerBound + porosity.UpperBound) / 2;

                _unitService.RegisterVariable(porosity.LowerBound);
                _unitService.RegisterVariable(porosity.UpperBound);
                _unitService.RegisterVariable(porosity.CurrentValue);

                Rock rock = new Rock
                {
                    Permeability = permeability,
                    Porosity = porosity
                };
                return Task.FromResult(new SuccessfulAction<Rock>(rock)
                    as IActionResult<Rock>);
            }
            catch (Exception e)
            {
                return Task.FromResult(new FailedResult<Rock>(
                    new List<string> { "Failed to read production data", e.Message })
                        as IActionResult<Rock>);
            }
        }

        private Task<IActionResult<Tank>> ReadTankData()
        {
            try
            {
                ISheet aquiferDataSheet = book.GetSheet(selectedSheet);
                int rowShift = 1;
                IRow row = aquiferDataSheet.GetRow(rowShift);
                var startOfProduction = row.GetCell(1).DateCellValue;

                row = aquiferDataSheet.GetRow(rowShift + 1);
                var flowingFluid = ConvertToFluidType(row.GetCell(1).StringCellValue);

                row = aquiferDataSheet.GetRow(rowShift + 2);
                var initialResevoirPressure = row.GetCell(1).NumericCellValue;

                row = aquiferDataSheet.GetRow(rowShift + 3);
                var connateWaterSaturation = row.GetCell(1).NumericCellValue;

                rowShift++;
                row = aquiferDataSheet.GetRow(rowShift + 4);
                var gasCap = new BoundedVariable<Dimensionless>
                {
                    LowerBound = row.GetCell(1).NumericCellValue,
                    UpperBound = row.GetCell(2).NumericCellValue
                };
                gasCap.CurrentValue = (gasCap.LowerBound + gasCap.UpperBound) / 2;

                _unitService.RegisterVariable(gasCap.LowerBound);
                _unitService.RegisterVariable(gasCap.UpperBound);
                _unitService.RegisterVariable(gasCap.CurrentValue);

                row = aquiferDataSheet.GetRow(rowShift + 5);
                var stoip = new BoundedVariable<OriginalOilInPlace>
                {
                    LowerBound = row.GetCell(1).NumericCellValue,
                    UpperBound = row.GetCell(2).NumericCellValue
                };
                stoip.CurrentValue = (stoip.LowerBound + stoip.UpperBound)/2;

                _unitService.RegisterVariable(stoip.LowerBound);
                _unitService.RegisterVariable(stoip.UpperBound);
                _unitService.RegisterVariable(stoip.CurrentValue);

                row = aquiferDataSheet.GetRow(rowShift + 6);
                var giip = new BoundedVariable<InitialGasInPlace>
                {
                    LowerBound = row.GetCell(1).NumericCellValue,
                    UpperBound = row.GetCell(2).NumericCellValue
                };
                giip.CurrentValue = (giip.LowerBound + giip.UpperBound) / 2;

                _unitService.RegisterVariable(giip.LowerBound);
                _unitService.RegisterVariable(giip.UpperBound);
                _unitService.RegisterVariable(giip.CurrentValue);

                row = aquiferDataSheet.GetRow(rowShift + 7);
                var thickness = new BoundedVariable<ReservoirThickness>
                {
                    LowerBound = row.GetCell(1).NumericCellValue,
                    UpperBound = row.GetCell(2).NumericCellValue
                };
                thickness.CurrentValue = (thickness.LowerBound + thickness.UpperBound) / 2;

                _unitService.RegisterVariable(thickness.LowerBound);
                _unitService.RegisterVariable(thickness.UpperBound);
                _unitService.RegisterVariable(thickness.CurrentValue);

                row = aquiferDataSheet.GetRow(rowShift + 8);
                var length = new BoundedVariable<ReservoirLength>
                {
                    LowerBound = row.GetCell(1).NumericCellValue,
                    UpperBound = row.GetCell(2).NumericCellValue
                };
                length.CurrentValue = (length.LowerBound + length.UpperBound) / 2;

                _unitService.RegisterVariable(length.LowerBound);
                _unitService.RegisterVariable(length.UpperBound);
                _unitService.RegisterVariable(length.CurrentValue);

                row = aquiferDataSheet.GetRow(rowShift + 9);
                var width = new BoundedVariable<ReservoirWidth>
                {
                    LowerBound = row.GetCell(1).NumericCellValue,
                    UpperBound = row.GetCell(2).NumericCellValue
                };
                width.CurrentValue = (width.LowerBound + width.UpperBound) / 2;

                _unitService.RegisterVariable(width.LowerBound);
                _unitService.RegisterVariable(width.UpperBound);
                _unitService.RegisterVariable(width.CurrentValue);

                row = aquiferDataSheet.GetRow(rowShift + 10);
                var radius = new BoundedVariable<ReservoirRadius>
                {
                    LowerBound = row.GetCell(1).NumericCellValue,
                    UpperBound = row.GetCell(2).NumericCellValue
                };
                radius.CurrentValue = (radius.LowerBound + radius.UpperBound) / 2;

                _unitService.RegisterVariable(radius.LowerBound);
                _unitService.RegisterVariable(radius.UpperBound);
                _unitService.RegisterVariable(radius.CurrentValue);

                var tank = new Tank
                {
                    StartOfProduction = startOfProduction,
                    FlowingFluid = flowingFluid,
                    InitialReservoirPressure = initialResevoirPressure,
                    ConnateWaterSaturation = connateWaterSaturation,

                    GasCap = gasCap,
                    STOIP = stoip,
                    GIIP = giip,
                    Thickness = thickness,
                    Length = length,
                    Width = width,
                    Radius = radius,
                };

                _unitService.RegisterVariable(tank.InitialReservoirPressure);
                _unitService.RegisterVariable(tank.ConnateWaterSaturation);

                return Task.FromResult(new SuccessfulAction<Tank>(tank)
                    as IActionResult<Tank>);
            }
            catch (Exception e)
            {
                return Task.FromResult(new FailedResult<Tank>(
                    new List<string> { "Failed to read tank data", e.Message })
                        as IActionResult<Tank>);
            }
        }

        private static FluidType ConvertToFluidType(string value)
        {
            return value.ToLower() switch
            {
                "oil" => FluidType.Oil,
                "gas" => FluidType.Gas,
                "condensate" => FluidType.Condensate,
                _ => throw new Exception("Fluid Type not valid"),
            };
        }

        private static WaterInfluxModel ConvertToWaterInfluxModel(string value)
        {
            return value.ToLower() switch
            {
                "hurst dake" => WaterInfluxModel.Hurst_Dake,
                "hurst_dake" => WaterInfluxModel.Hurst_Dake,
                "hurst modified" => WaterInfluxModel.Hurst_Modified,
                "hurst_modified" => WaterInfluxModel.Hurst_Modified,
                "carter tracy" => WaterInfluxModel.CaterTracy,
                "carter_tracy" => WaterInfluxModel.CaterTracy,
                "fetkovich" => WaterInfluxModel.Fetkovich,
                _ => throw new Exception("Water influx model not valid"),
            };
        }

        private static BoundaryCondition ConvertToBoundary(string value)
        {
            return (value.ToLower()) switch
            {
                "closed boundary" => BoundaryCondition.Closed_Boundary,
                "constant pressure" => BoundaryCondition.Constant_Pressure_Boundary,
                "infinite acting" => BoundaryCondition.Infinite_Acting,
                _ => throw new Exception("Boundary condition not valid"),
            };
        }

        private static Geometry ConvertToGeometry(string value)
        {
            return (value.ToLower()) switch
            {
                "linear" => Geometry.Linear,
                "radial" => Geometry.Radial,
                _ => throw new Exception("Geometry not valid"),
            };
        }

        private static Position ConvertToPosition(string value)
        {
            return (value.ToLower()) switch
            {
                "edge" => Position.Edge,
                "bottom" => Position.Bottom,
                _ => throw new Exception("Fluid Type not valid"),
            };
        }

        private IActionResult SetupWorkbook()
        {
            var excelImportDialog = Ioc.Resolve<IExcelImportDialog>();

            if (excelImportDialog.ShowAsDialog() != true) return new FailedResult();

            if (excelImportDialog.Workbook != null)
            {
                book = excelImportDialog.Workbook;
                selectedSheet = excelImportDialog.SelectedSheet;
                return new SuccessfulAction();
            }
            return new FailedResult();
        }

        public void Dispose() => book?.Close();
    }
}
