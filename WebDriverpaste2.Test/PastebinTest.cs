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

        private string website_url = "https://pastebin.com/ ";

        private string postform_text = "git config --global user.name  \"New Sheriff in Town\"\ngit reset $(git commit-tree HEAD^{tree} -m \"Legacy code\")\ngit push origin master --force";

        private string post_name = "how to gain dominance among developers";

        [SetUp]
        public void SetUp()
        {
            webDriver = new ChromeDriver();
        }

        [Test]
        public void CreateNewPasteTest()
        {
            webDriver.Navigate().GoToUrl(website_url);

            webDriver.FindElement(By.Id("postform-text")).SendKeys(postform_text);

            webDriver.FindElement(By.Id("select2-postform-format-container")).Click();

            By syntaxDropdownSelector = By.XPath("//li[@class='select2-results__option' and starts-with(@id, 'select2-postform-format-result-')]");
            new WebDriverWait(webDriver, TimeSpan.FromSeconds(10)).Until(driver => driver.FindElement(syntaxDropdownSelector).Displayed);

            IWebElement optionBash = webDriver.FindElement(syntaxDropdownSelector);
            optionBash.Click();

            By expirationDropdownSelector = By.Id("select2-postform-expiration-container");
            new WebDriverWait(webDriver, TimeSpan.FromSeconds(10)).Until(driver => driver.FindElement(expirationDropdownSelector).Displayed);

            IWebElement option10Minutes = webDriver.FindElement(expirationDropdownSelector);
            option10Minutes.Click();

            webDriver.FindElement(By.Id("postform-name")).SendKeys(post_name);

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

            new WebDriverWait(webDriver, TimeSpan.FromSeconds(10)).Until(driver => driver.Title.StartsWith(post_name));

            Assert.That(webDriver.FindElement(By.CssSelector(".info-top h1")).Text.Trim(), Is.EqualTo(post_name));
        }

        [TearDown]
        public void TearDown()
        {
            webDriver.Quit();
        }
    }
}