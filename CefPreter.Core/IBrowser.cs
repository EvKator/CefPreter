using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CefPreter
{
    public interface IBrowser
    {
        

        Task Navigate(string url);
        

        Task Click(string xpath);

        Task Enter(string xpath, string text);

        Task WaitForElement(string xpath, int interval = 1000, int timeout = 600000);

        Task Wait(int n);

        Task Reload();

        Task GoBack();

        Task GoForward();

        Task<string> GetInnerHTML(string xpath);
    }
}
