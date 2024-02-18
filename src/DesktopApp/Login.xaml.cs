using Microsoft.Web.WebView2.Core;
using System;
using System.Buffers.Text;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Windows;

namespace DesktopApp
{
    public class CodeReceivedEventArgs : EventArgs
    {
        public CodeReceivedEventArgs(string code, string codeVerifier) { 
            Code = code;
            CodeVerifier = codeVerifier;
        }

        public string Code { get; }
        public string CodeVerifier { get; }
    }

    public partial class Login : Window
    {
        private string codeVerifier;
        public Login()
        {
            InitializeComponent();

            Loaded += Login_Loaded;
        }

        public event EventHandler<CodeReceivedEventArgs>? CodeReceived;

        private void Login_Loaded(object sender, RoutedEventArgs e)
        {
            WebView.CoreWebView2InitializationCompleted += WebView_CoreWebView2InitializationCompleted;

            codeVerifier = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
            var loginUrl = CasdoorVariables.casdoorClient.GetSigninUrl(codeVerifier, true);
            WebView.Source = new Uri(loginUrl);
        }

        private void WebView_CoreWebView2InitializationCompleted(object? sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            var uri = new Uri(CasdoorVariables.Domain);
            WebView.CoreWebView2.CookieManager.DeleteCookiesWithDomainAndPath("casdoor_session_id", uri.Host, "/");
            WebView.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;
        }

        private void CoreWebView2_NewWindowRequested(object? sender, CoreWebView2NewWindowRequestedEventArgs e)
        {
            string GetCodeFromUrl(string url) 
                => HttpUtility.ParseQueryString(new Uri(url).Query).Get("code")!;

            var deferral = e.GetDeferral();
            string? code = null;

            if (e.Uri.StartsWith("casdoor://", StringComparison.OrdinalIgnoreCase))
            {
                code = GetCodeFromUrl(e.Uri);
                CodeReceived?.Invoke(this, new CodeReceivedEventArgs(code, codeVerifier));
            }

            deferral.Complete();
            e.Handled = true;

            if (code != null)
                Close();
        }
    }
}
