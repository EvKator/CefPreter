using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CefSharp;
using CefSharp.Wpf;

namespace CefPreter.Browser
{
    public class Browser: CefPreter.IBrowser
    {
        static bool Processing = false;
        public ChromiumWebBrowser Chromium { get; set; }

        public Browser()
        {
            //Processing = true;

        }


        public async Task Navigate(string url)
        {

            Processing = true;
            Chromium.Stop();
            if(Chromium.Address != url)
            {
                Chromium.Address = url;
                Chromium.LoadingStateChanged += (sender, args) =>
                {
                    if (args.IsLoading == false)
                    {
                        Processing = false;
                    }
                };
                await WaitForProcessing();
            }
            
        }

        public async Task<JavascriptResponse> GetElementByXpath(string xpath)
        {
            return await Chromium.EvaluateScriptAsync("(function(){window.foundEl = document.evaluate(\"" + xpath + "\", document, null, XPathResult.ORDERED_NODE_SNAPSHOT_TYPE, null).snapshotItem(0); return window.foundEl; })();");
        }

        public async Task Click(string xpath)
        {
            JavascriptResponse obj = await GetElementByXpath(xpath);
            await Chromium.EvaluateScriptAsync("window.foundEl.click()");

        }

        public async Task Enter(string xpath, string text)
        {
            await GetElementByXpath(xpath);
            await Chromium.EvaluateScriptAsync("window.foundEl.value = \"" + text + "\"");
            await Chromium.EvaluateScriptAsync("(function(){var event = new Event(\"focus\");" +
                                        "window.foundEl.dispatchEvent(event); window.foundEl.focus(); window.foundEl.click(); })()");
        }

        public async Task WaitForElement(string xpath, int interval = 1000, int timeout = 600000)
        {
            Processing = true;
            while (Processing && timeout >= 0)
            {
                await Task.Delay(interval);
                timeout -= interval;
                string res = await GetInnerHTML(xpath);
                if (res != null)
                    Processing = false;
            }
            if (timeout <= 0)
                throw new Exception("WaitForElement timeout expired");

        }

        public async Task Wait(int n)
        {
            await Task.Delay(n);
        }

        public async Task Reload()
        {
            Chromium.Reload(true);
        }

        public async Task GoBack()
        {
            Chromium.Back();
        }

        public async Task GoForward()
        {
            Chromium.Forward();
        }

        public async Task<string> GetInnerHTML(string xpath)
        {
            // await GetElementByXpath(xpath);;
            //JavascriptResponse res = await browser.EvaluateScriptAsync("(function(){return window.innerHTML; })()");

            JavascriptResponse res = await Chromium.EvaluateScriptAsync("(function(){return document.evaluate('" + xpath + "', document, null, XPathResult.ORDERED_NODE_SNAPSHOT_TYPE, null).snapshotItem(0).innerHTML; })();");

            return (string)res.Result;
        }

        private async Task WaitForProcessing(int interval = 1000, int timeout = 100000)
        {
            while (Processing && timeout >= 0)
            {
                await Task.Delay(interval);
                timeout -= interval;
            }
            if (timeout <= 0)
                throw new Exception("timeout expired");
        }

    }
}
