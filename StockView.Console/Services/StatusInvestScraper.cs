using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using StockView.Console.Entities;

namespace StockView.Console.Services;

public class StatusInvestScraper : IDisposable
{
    private const string BaseUri = @"https://statusinvest.com.br/acoes";
    private readonly ChromeDriver _driver;
    private readonly WebDriverWait _wait;

    public StatusInvestScraper()
    {
        _driver = new ChromeDriver();
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
    }

    public void Dispose()
    {
        _driver?.Quit();
        _driver?.Dispose();
    }

    public Stock GetDataStockByName(string stockCode)
    {
        try
        {
            _driver.Navigate().GoToUrl($"{BaseUri}/{stockCode}");
            _driver.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");

            var url = GetElementAttribute(By.CssSelector(
                    "#company-section > div:nth-child(1) > div > div.d-block.d-md-flex.mb-5.img-lazy-group > div.company-description.w-100.w-md-70.ml-md-5 > span > a"),
                "href");

            var roic = GetElementValueAsDouble(By.CssSelector(
                "#indicators-section > div.indicator-today-container > div > div:nth-child(4) > div > div:nth-child(3) > div > div > strong"));

            var revenueCagr5Years = GetElementValueAsDouble(By.CssSelector(
                "#indicators-section > div.indicator-today-container > div > div:nth-child(5) > div > div:nth-child(1) > div > div > strong"));

            var averagePayout = GetElementValueAsDouble(By.CssSelector(
                "#payout-section > div > div > div.d-md-flex.justify-between.align-items-center.mb-2.mb-lg-4 > div.values.d-flex.flex-wrap.justify-around.flex-sm-nowrap.align-items-center.w-100.w-md-auto > div:nth-child(1) > strong"));

            var netDebtToEbitda = GetElementValueAsDouble(By.CssSelector(
                "#indicators-section > div.indicator-today-container > div > div:nth-child(2) > div > div:nth-child(2) > div > div > strong"));

            var pVp = GetElementValueAsDouble(By.CssSelector(
                "#indicators-section > div.indicator-today-container > div > div:nth-child(1) > div > div:nth-child(4) > div > div > strong"));


            var stock = new Stock
            {
                Url = url,
                Code = stockCode.ToUpper(),
                Roic = roic,
                RevenueCagr5Years = revenueCagr5Years,
                AveragePayout = averagePayout,
                NetDebtToEbitda = netDebtToEbitda,
                PVp = pVp
            };

            return stock;
        }
        catch (Exception e)
        {
            System.Console.WriteLine("Invalid stock code");
            System.Console.WriteLine(e.Message);
            throw;
        }
    }

    private string GetElementAttribute(By by, string attribute)
    {
        _wait.Until(d => d.FindElement(by).GetAttribute(attribute) != "-");
        return _driver.FindElement(by).GetAttribute(attribute);
    }

    private double GetElementValueAsDouble(By by)
    {
        _wait.Until(d => d.FindElement(by).Text != "-");
        var text = _driver.FindElement(by).Text.Replace(",", ".").Replace("%", "");
        return double.Parse(text);
    }
}