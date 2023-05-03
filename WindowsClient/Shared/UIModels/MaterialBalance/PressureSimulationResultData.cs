using System.Collections.Generic;

namespace WindowsClient.Shared.UIModels.MaterialBalance
{
    public class PressureSimulationResultData : List<PressureSimulationResultRow>
    {
        public TimeSeries GetPressureData()
        {
            var series = new TimeSeries(true);
            ForEach(row => series.Add((row.Time, row.TankPressure)));
            return series;
        }
        public TimeSeries GetCummulativeOilProducedData()
        {
            var series = new TimeSeries(true);
            ForEach(row => series.Add((row.Time, row.CummulativeOilProduced)));
            return series;
        }
        public TimeSeries GetCummulativeGasProducedData()
        {
            var series = new TimeSeries(true);
            ForEach(row => series.Add((row.Time, row.CummulativeGasProduced)));
            return series;
        }
        public TimeSeries GetCummulativeWaterProducedData()
        {
            var series = new TimeSeries(true);
            ForEach(row => series.Add((row.Time, row.CummulativeWaterProduced)));
            return series;
        }
        public TimeSeries GetAquiferInfluxData()
        {
            var series = new TimeSeries(true);
            ForEach(row => series.Add((row.Time, row.AquiferInflux)));
            return series;
        }
        public TimeSeries GetAverageGasInjectionRateData()
        {
            var series = new TimeSeries(true);
            ForEach(row => series.Add((row.Time, row.AverageGasInjectionRate)));
            return series;
        }
        public TimeSeries GetAverageGasRateData()
        {
            var series = new TimeSeries(true);
            ForEach(row => series.Add((row.Time, row.AverageGasRate)));
            return series;
        }
        public TimeSeries GetAverageLiquidRateData()
        {
            var series = new TimeSeries(true);
            ForEach(row => series.Add((row.Time, row.AverageLiquidRate)));
            return series;
        }
        public TimeSeries GetAverageWaterRateData()
        {
            var series = new TimeSeries(true);
            ForEach(row => series.Add((row.Time, row.AverageWaterRate)));
            return series;
        }
        public TimeSeries GetAverageOilRateData()
        {
            var series = new TimeSeries(true);
            ForEach(row => series.Add((row.Time, row.AverageOilRate)));
            return series;
        }
        public TimeSeries GetAverageWaterInjectionRateData()
        {
            var series = new TimeSeries(true);
            ForEach(row => series.Add((row.Time, row.AverageWaterInjectionRate)));
            return series;
        }
        public TimeSeries GetCummulativeGasInjectedData()
        {
            var series = new TimeSeries(true);
            ForEach(row => series.Add((row.Time, row.CummulativeGasInjected)));
            return series;
        }
        public TimeSeries GetCummulativeWaterInjectedData()
        {
            var series = new TimeSeries(true);
            ForEach(row => series.Add((row.Time, row.CummulativeWaterInjected)));
            return series;
        }
        public TimeSeries GetOilDensityData()
        {
            var series = new TimeSeries(true);
            ForEach(row => series.Add((row.Time, row.OilDensity)));
            return series;
        }
        public TimeSeries GetOilFVFData()
        {
            var series = new TimeSeries(true);
            ForEach(row => series.Add((row.Time, row.OilFVF)));
            return series;
        }
        public TimeSeries GetOilRecoveryFactorData()
        {
            var series = new TimeSeries(true);
            ForEach(row => series.Add((row.Time, row.OilRecoveryFactor)));
            return series;
        }
        public TimeSeries GetOilViscosityData()
        {
            var series = new TimeSeries(true);
            ForEach(row => series.Add((row.Time, row.OilViscosity)));
            return series;
        }
        public TimeSeries GetProducingCGRData()
        {
            var series = new TimeSeries(true);
            ForEach(row => series.Add((row.Time, row.ProducingCGR)));
            return series;
        }
        public TimeSeries GetProducingGORData()
        {
            var series = new TimeSeries(true);
            ForEach(row => series.Add((row.Time, row.ProducingGOR)));
            return series;
        }

        public TimeSeries GetOilWaterContactData()
        {
            var series = new TimeSeries(true);
            ForEach(row => series.Add((row.Time, row.OilWaterContact)));
            return series;
        }

        public TimeSeries GetGasOilContactData()
        {
            var series = new TimeSeries(true);
            ForEach(row => series.Add((row.Time, row.GasOilContact)));
            return series;
        }
    }
}
