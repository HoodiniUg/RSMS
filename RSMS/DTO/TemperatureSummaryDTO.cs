namespace RSMS.DTO
{
    public class TemperatureSummaryDTO
    {
        public double AvgTemperature { get; set; }
        public double MinTemperature { get; set; }
        public double MaxTemperature { get; set; }
        public string SensorStatus { get; set; } = string.Empty;
    }
}
