using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using UI_V2.ResourceHelper;

namespace UI_V2.WebView
{
    public class WebViewEx
    {
        private readonly WebView2 _webView;

        private readonly string _contentUri;
        private readonly string _htmlData;

        private readonly eContentSource _contentSource;

        private event EventHandler OnReady = null;
        private event EventHandler<OnMessageEventArgs> OnMessage = null;

        public WebViewEx(WebView2 webView, string contentUri = null, string htmlData = null, EventHandler onReady = null, EventHandler<OnMessageEventArgs> onMessage = null)
        {
            _webView = webView;

            _contentUri = contentUri;
            _htmlData = htmlData;

            if (_contentUri != null)
                _contentSource = eContentSource.URI;
            else if (_htmlData != null)
                _contentSource = eContentSource.HTML;
            else
                throw new Exception("No content source was supplied!");

            if (onReady != null)
                OnReady += onReady;

            if (onMessage != null)
                OnMessage += onMessage;
        }

        #region Class Types

        public class OnMessageEventArgs : EventArgs
        {
            public readonly string Message;

            public OnMessageEventArgs(string message)
            {
                Message = message;
            }
        }

        enum eContentSource
        {
            HTML,
            URI
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Initializes the WebView with the data and options supplied in .ctor().
        /// </summary>
        /// <param name="isNonClientRegionSupportEnabled"></param>
        public async void Initialize(bool isNonClientRegionSupportEnabled = false)
        {
            await _webView.EnsureCoreWebView2Async();

            WebViewUtils.SetOptions(_webView, isNonClientRegionSupportEnabled);

            _webView.WebMessageReceived += OnWebMessageReceived;
            _webView.NavigationCompleted += OnNavigationCompleted;

            LoadContentSource();

#if DEBUG
            _webView.CoreWebView2.OpenDevToolsWindow();
#endif
        }

        /// <summary>
        /// Sends a string to the WebView.
        /// JS docs: https://learn.microsoft.com/en-us/microsoft-edge/webview2/reference/javascript/webview#webview2script-webview-addeventlistener-member(2)
        /// </summary>
        public void SendString(string message)
        {
            _webView.CoreWebView2?.PostWebMessageAsString(message);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// (re)loads the content of the WebView.
        /// </summary>
        private void LoadContentSource()
        {
            if (_contentSource == eContentSource.HTML)
                _webView.CoreWebView2.NavigateToString(_htmlData);
            else if (_contentSource == eContentSource.URI)
                _webView.CoreWebView2.Navigate(_contentUri);
        }

        /// <summary>
        /// The event that is invoked when the WebView sends a string/json to the c# side.
        /// </summary>
        private void OnWebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            string message = e.WebMessageAsJson;

            string plainMessage = WebViewUtils.GetPlainStringFromJsonMessage(message);
            if (plainMessage == "DOMContentLoaded")
            {
                OnReady?.Invoke(this, EventArgs.Empty);
            }
            else if (plainMessage == "reload")
                LoadContentSource();
            else
                OnMessage?.Invoke(this, new(message));
        }

        /// <summary>
        /// Called any time navigation occurs on the page.
        /// </summary>
        private void OnNavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            if (!e.IsSuccess)
            {
                WebResourceLoader loader = new("www.ErrorPage.ErrorPage.html");
                _webView.CoreWebView2.NavigateToString(loader.GetText());
            }
        }

        #endregion
    }
}
