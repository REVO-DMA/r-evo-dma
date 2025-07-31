using Microsoft.Web.WebView2.WinForms;

namespace UI_V2.WebView
{
    public static class WebViewUtils
    {
        /// <summary>
        /// Applies a default set of options to the supplied WebView.
        /// These default options are well suited for a desktop app and disable lots of web-like behavior.
        /// </summary>
        public static void SetOptions(WebView2 webView, bool isNonClientRegionSupportEnabled = false)
        {
            webView.CoreWebView2.Settings.AreBrowserAcceleratorKeysEnabled = false;
            webView.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
            webView.CoreWebView2.Settings.AreDefaultScriptDialogsEnabled = false;
            webView.CoreWebView2.Settings.AreHostObjectsAllowed = true;
            webView.CoreWebView2.Settings.AreDevToolsEnabled = false;

            webView.CoreWebView2.Settings.IsNonClientRegionSupportEnabled = isNonClientRegionSupportEnabled;
            webView.CoreWebView2.Settings.IsReputationCheckingRequired = false;
            webView.CoreWebView2.Settings.IsSwipeNavigationEnabled = false;
            webView.CoreWebView2.Settings.IsBuiltInErrorPageEnabled = false;
            webView.CoreWebView2.Settings.IsGeneralAutofillEnabled = false;
            webView.CoreWebView2.Settings.IsPasswordAutosaveEnabled = false;
            webView.CoreWebView2.Settings.IsPinchZoomEnabled = false;
            webView.CoreWebView2.Settings.IsZoomControlEnabled = false;
        }

        /// <summary>
        /// Converts a json string into a plain string by removing the surrounding double quotes.
        /// </summary>
        public static string GetPlainStringFromJsonMessage(string jsonMessage)
        {
            string msg = jsonMessage.Trim('"');

            return msg;
        }
    }
}
