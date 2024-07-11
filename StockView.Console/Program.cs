using StockView.Console.Services;

var statusInvestScraper = new StatusInvestScraper();
var stock = statusInvestScraper.GetDataStockByName("vale3");

