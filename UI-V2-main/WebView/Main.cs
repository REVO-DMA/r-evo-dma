using Microsoft.Web.WebView2.WinForms;

namespace UI_V2.WebView
{
    public class Main
    {
        private readonly Window _window;
        private readonly WebView2 _webView;
        private readonly WebViewEx _webViewEx;

        public Main(Window window, WebView2 webView)
        {
            _window = window;
            _webView = webView;

#if DEBUG
            string contentUri = "http://127.0.0.1:8080";
#else
            string contentUri = "https://cdn.evodma.com/eft-ui";
#endif
            _webViewEx = new(_webView, contentUri: contentUri);

            _webViewEx.Initialize();
        }
    }
}
