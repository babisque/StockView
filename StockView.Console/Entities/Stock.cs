namespace StockView.Console.Entities;

public class Stock
{
    public string? Url { get; set; }
    public string? Code { get; set; }
    public double? Roic { get; set; }
    public double? RevenueCagr5Years { get; set; }
    public double? AveragePayout { get; set; }
    public double? NetDebtToEbitda { get; set; }
    public double? PVp { get; set; }
}