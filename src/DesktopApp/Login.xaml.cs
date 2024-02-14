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
        public CodeReceivedEventArgs(string code) => Code = code;

        public string Code { get; }
    }

    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();

            Loaded += Login_Loaded;
        }

        public event EventHandler<CodeReceivedEventArgs>? CodeReceived;

        private void Login_Loaded(object sender, RoutedEventArgs e)
        {
            WebView.CoreWebView2InitializationCompleted += WebView_CoreWebView2InitializationCompleted;

            var loginUrl = GetLoginUrl();
            WebView.Source = new Uri(loginUrl);
        }

        private void WebView_CoreWebView2InitializationCompleted(object? sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            WebView.CoreWebView2.CookieManager.DeleteCookiesWithDomainAndPath("casdoor_session_id", "door.casdoor.com", "/");
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
                CodeReceived?.Invoke(this, new CodeReceivedEventArgs(code));
            }

            deferral.Complete();
            e.Handled = true;

            if (code != null)
                Close();
        }

        private string GetLoginUrl()
        {
            var sha256Instance = SHA256.Create();
            byte[] bytes = Encoding.Default.GetBytes(CasdoorVariables.ChallengeCode);
            byte[] chanllengeCodeEncoded = sha256Instance.ComputeHash(bytes);
            string chanllengeCodeBase64Encoded = Convert.ToBase64String(chanllengeCodeEncoded).Replace("+", "-").Replace("/", "_").Replace("=",""); 
            
            return $"{CasdoorVariables.Domain}/login/oauth/authorize?client_id={CasdoorVariables.ClientId}&response_type=code&redirect_uri={CasdoorVariables.CallbackUrl}&scope=profile&state={CasdoorVariables.AppName}&noRedirect=true&code_challenge={chanllengeCodeBase64Encoded}&code_challenge_method=S256";
        }
           
    }
}
