# CefPreter
Interpreter, makes it possible to quickly and easily create bots on C#.

## Basic Information


CefPreter constructed as WPF component and has a built-in browser, in the future also a low-level library for http requestsing. For working with page data it uses XPath

  Main advantage of CefPreter - extensibility.
  It's very easy to create own functions: just make a class like "ChangeElementTextFunction", inherit it from abstract class Function (CefPreter.Function), compile, save as "Function.dll" in main folder of CefIDE.

  Currently realized functions: Print, AString, ANumber, ToStr, ToNumber, IsGreater, IsLess, AreEqual, Navigate, Click, Enter, GoBack, GoForward, Reload, Wait, WaitForElement, If, While, InnerHTML, CLBCK, Beep

The solution separated to 4 parts : Function, Browser, CefPreter, CefPreter.Core.
Function defines functions, used in CefPreter, Browser - methods for interacting with browser. CefPreter.Core difines main types and interfaces, which extended or implemented in other parts of the solution.
So you can to create your implementation of Function, Browser and connect them to the project.

CefPreter has own memory scope, where all internal variables used by interpreter are located (CefMemory).
One of realized functions - CLBCK (Callback) emmits CefPreter event CallBack and CallbackEventHandler(CefMemory) delegate and makes it possible to read all current internal variables of interpreter from your C# project, so you can to use CefPreter just for working with internet pages, then get the results and handle it in the main program.

  ## Example #0: 
 The main example of using CefPreter library - <a href="https://github.com/EvKator/CefIDE">CefIDE</a>

## Example #1: 
```
public void Run(){
	CefPreter cef = new CefPreter();
	cef.code = "Begin While 1; Begin AString percent InnerHTML '//div[@class="trading-data m-unit-bet-opinions-most"]/div/span[1]'; CLBCK; End End";
	cef.Callback+=(CefMemory mem) => MessageBox.Show((string)mem.Get("percent").Value);
	cef.Run();
}
```

## Example #2
Demonstration of working CefPreter, for clarity in <a href="https://github.com/EvKator/CefIDE">CefIDE</a>
The bot navigates to the binary option and starts parsing the percentage of people means the rate will rise and prints it. 
If the percentage is greater than 70, it makes a sound and prints the message "PERCENT GREATER THAN 70!!!".That is, it helps at the right time (depending on your conditions) to buy / sell shares.


### GIF:

![](Example.gif)

### Code:

```
Begin
ANumber interval 2000;

Print Navigate 'https://binomo.com/ru/trading';
WaitForElement '//div[@class="trading-data m-unit-bet-opinions-most"]/div/span[1]';

While 1;
Begin
AString percent InnerHTML '//div[@class="trading-data m-unit-bet-opinions-most"]/div/span[1]';

If IsGreater ToNumber percent 70;
Begin
Print 'PERCENT GREATER THAN 70!!!';
Beep;
End

Print percent;
Wait interval;
End
End
```
