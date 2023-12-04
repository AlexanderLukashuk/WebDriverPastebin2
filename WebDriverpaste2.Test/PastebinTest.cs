using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace WebDriverpaste2.Test
{
    public class PastebinTest
    {
        private IWebDriver webDriver;

        [SetUp]
        public void SetUp()
        {
            webDriver = new ChromeDriver();
        }

        [Test]
        public void CreateNewPasteTest()
        {
            webDriver.Navigate().GoToUrl("https://pastebin.com/ ");

            webDriver.FindElement(By.Id("postform-text")).SendKeys("git config --global user.name  \"New Sheriff in Town\"\ngit reset $(git commit-tree HEAD^{tree} -m \"Legacy code\")\ngit push origin master --force");

            webDriver.FindElement(By.Id("select2-postform-format-container")).Click();

            By syntaxDropdownSelector = By.XPath("//li[@class='select2-results__option' and starts-with(@id, 'select2-postform-format-result-')]");
            new WebDriverWait(webDriver, TimeSpan.FromSeconds(10)).Until(driver => driver.FindElement(syntaxDropdownSelector).Displayed);

            IWebElement optionBash = webDriver.FindElement(syntaxDropdownSelector);
            optionBash.Click();

            By expirationDropdownSelector = By.Id("select2-postform-expiration-container");
            new WebDriverWait(webDriver, TimeSpan.FromSeconds(10)).Until(driver => driver.FindElement(expirationDropdownSelector).Displayed);

            IWebElement option10Minutes = webDriver.FindElement(expirationDropdownSelector);
            option10Minutes.Click();

            webDriver.FindElement(By.Id("postform-name")).SendKeys("how to gain dominance among developers");

            By submitButtonSelector = By.CssSelector("button[class='btn -big']");
            new WebDriverWait(webDriver, TimeSpan.FromSeconds(10)).Until(driver =>
            {
                try
                {
                    var submitButton = webDriver.FindElement(submitButtonSelector);
                    return submitButton.Enabled && submitButton.Displayed;
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            webDriver.FindElement(submitButtonSelector).Click();

            new WebDriverWait(webDriver, TimeSpan.FromSeconds(10)).Until(driver => driver.Title.StartsWith("how to gain dominance among developers"));

            // By titleSelector = By.XPath("//div[@class='info-top']/h1");
            Assert.AreEqual("how to gain dominance among developers", webDriver.FindElement(By.CssSelector(".info-top h1")).Text.Trim());
        }

        [TearDown]
        public void TearDown()
        {
            webDriver.Quit();
        }
    }
}