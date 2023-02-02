# BlazorTWAIN

Document Scanner using [Dynamic Web TWAIN (DWT)](https://www.dynamsoft.com/web-twain/overview/) and Blazor

There are two examples: the Basic example and the OCR+Barcode example.

The Basic example just uses DWT to scan documents. The OCR+Barcode example adds barcode reading and OCR functions to the Basic example. Both server and webassembly versions are provided.

Additional steps to run the projects.

1. Download [Dynamic Web TWAIN](https://www.dynamsoft.com/web-twain/downloads/) and copy its `Resources` folder to the `wwwroot` folder.
2. Modify `dynamsoft.webtwain.config.js` in the `Resources` folder. Set up `ProductKey` and change `Dynamsoft.DWT.AutoLoad` to false as we want to load DWT manually. You can apply for a free trial license key [here](https://www.dynamsoft.com/customer/license/trialLicense/?product=dwt).
3. (optional) You may need to install Windows 10 SDK if you want to use the [windows.media.ocr](https://docs.microsoft.com/en-us/uwp/api/Windows.Media.Ocr.OcrEngine?view=winrt-20348) API. Visit [here](https://docs.microsoft.com/en-us/windows/apps/desktop/modernize/desktop-to-uwp-enhance) to learn more about using Windows Runtime API in .Net applications.

## Blog

[How to Build a Web Document Scanner with Blazor](https://www.dynamsoft.com/codepool/web-document-scanner-with-blazor.html)

## Blazor in a Desktop App

There is also a demo running Dynamic Web TWAIN in a WinForms desktop app using BlazorWebView: [Combine Blazor and WinForms to Build a Document Scanning Desktop App](https://www.dynamsoft.com/codepool/blazor-webview-winforms-document-scanner.html)



