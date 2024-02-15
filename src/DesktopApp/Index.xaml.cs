using System.Windows;

namespace DesktopApp
{
    public partial class Index : Window
    {
        private readonly CasdoorApi _casdoorApi;
        private string? _authCode;
        private string? _authCodeVerifier;

        public Index()
        {
            InitializeComponent();

            _casdoorApi = new CasdoorApi(CasdoorVariables.Domain);
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            var login = new Login();
            login.CodeReceived += Login_CodeReceived;

            login.ShowDialog();
        }

        private void LogoutBtn_Click(object sender, RoutedEventArgs e)
        {
            _authCode = null;
            Username.Text = string.Empty;

            UserProfilePnl.Visibility = Visibility.Collapsed;
            LoginPnl.Visibility = Visibility.Visible;
        }

        private async void Login_CodeReceived(object? sender, CodeReceivedEventArgs e)
        {
            _authCode = e.Code;
            _authCodeVerifier = e.CodeVerifier;

            UserProfilePnl.Visibility = Visibility.Visible;
            LoginPnl.Visibility = Visibility.Collapsed;
            Username.Text = "Loading...";

            var token = await _casdoorApi.RequestToken(
                CasdoorVariables.ClientId,
                _authCode,
                _authCodeVerifier
            );

            // Assume request token and get user process is in happy path..
            var user = await _casdoorApi.GetUserInfo(token!);
            Username.Text = user!.Name;
        }
    }
}
