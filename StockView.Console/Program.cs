using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using StockView.Console;

const string uri = @"https://statusinvest.com.br/acoes";
var stockCode = "vale3";

using var driver = new ChromeDriver();
driver.Navigate().GoToUrl($"{uri}/{stockCode}");
driver.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
    
// need to wait to load all information
var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

var url = driver.FindElement(By.CssSelector(
    "#company-section > div:nth-child(1) > div > div.d-block.d-md-flex.mb-5.img-lazy-group > div.company-description.w-100.w-md-70.ml-md-5 > span > a")).GetAttribute("href");

var roic = driver
    .FindElement(By.CssSelector(
        "#indicators-section > div.indicator-today-container > div > div:nth-child(4) > div > div:nth-child(3) > div > div > strong"))
    .Text;

var cagr5Years = driver
    .FindElement(By.CssSelector(
        "#indicators-section > div.indicator-today-container > div > div:nth-child(5) > div > div:nth-child(1) > div > div > strong"))
    .Text;

var payoutAvg = driver.FindElement(By.CssSelector(
        "#payout-section > div > div > div.d-md-flex.justify-between.align-items-center.mb-2.mb-lg-4 > div.values.d-flex.flex-wrap.justify-around.flex-sm-nowrap.align-items-center.w-100.w-md-auto > div:nth-child(1) > strong"))
    .Text;

var divLiqEbitada = driver
    .FindElement(By.CssSelector(
        "#indicators-section > div.indicator-today-container > div > div:nth-child(2) > div > div:nth-child(2) > div > div > strong"))
    .Text;

var pVp = driver
    .FindElement(By.CssSelector(
        "#indicators-section > div.indicator-today-container > div > div:nth-child(1) > div > div:nth-child(4) > div > div > strong"))
    .Text;

wait.Until(d => url != "-");
wait.Until(d => roic != "-");
wait.Until(d => cagr5Years != "-");
wait.Until(d => payoutAvg != "-");
wait.Until(d => divLiqEbitada != "-");
wait.Until(d => pVp != "-");


var stock = new Stock
{
    Url = url,
    Cod = stockCode.ToUpper(),
    Roic = roic,
    Cagr5Years = cagr5Years,
    PayoutAvg = payoutAvg,
    DivLiqEbitada = divLiqEbitada,
    PVp = pVp
};