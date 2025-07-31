using Microsoft.Web.WebView2.WinForms;
using UI_V2.ResourceHelper;

namespace UI_V2.WebView
{
    public class TopBar
    {
        private readonly Window _window;
        private readonly WebView2 _webView;
        private readonly WebViewEx _webViewEx;

        private readonly string _htmlData;

        public TopBar(Window window, WebView2 webView)
        {
            _window = window;
            _webView = webView;

            WebResourceLoader loader = new("www.TopBar.TopBar.html");
            _htmlData = loader.GetText();

            _webViewEx = new(_webView, htmlData: loader.GetText(), onReady: OnReady, onMessage: OnMessage);

            _webViewEx.Initialize(true);
        }

        #region Public Methods

        public void SetFocus() => _webViewEx.SendString("focus");

        public void SetBlur() => _webViewEx.SendString("blur");

        public void SetMaximized() => _webViewEx.SendString("maximize");

        public void SetRestored() => _webViewEx.SendString("restore");

        #endregion

        #region Private Methods

        private void OnMessage(object sender, WebViewEx.OnMessageEventArgs e)
        {
            string msg = WebViewUtils.GetPlainStringFromJsonMessage(e.Message);

            if (msg == "minimize")
                _window.Minimize();
            else if (msg == "maximizeRestore")
            {
                if (_window.IsMaximized())
                    _window.Restore();
                else
                    _window.Maximize();
            }
            else if (msg == "close")
                Application.Exit();
            else if (msg == "restart")
                _webView.CoreWebView2.NavigateToString(_htmlData);
        }

        private void OnReady(object sender, EventArgs e)
        {
            if (_window.IsMaximized())
                SetMaximized();
            else
                SetRestored();
        }

        #endregion
    }
}
