using System.Windows;

namespace DesktopApp
{
    public partial class UserProfile : Window
    {
        private readonly string _authCode;
        private readonly CasdoorApi _casdoorApi;

        public UserProfile(string authCode)
        {
            InitializeComponent();

            _authCode = authCode;
            _casdoorApi = new CasdoorApi(CasdoorVariables.Domain);

            Loaded += UserProfile_Loaded;
        }

        private async void UserProfile_Loaded(object sender, RoutedEventArgs e)
        {
            var token = await _casdoorApi.RequestToken(
                CasdoorVariables.ClientId, 
                CasdoorVariables.ClientSecret, 
                _authCode
            );

            // Assume request token and get user process is in happy path..
            var user = await _casdoorApi.GetUserInfo(token!);
            Username.Text = user!.Name;
        }

        private void LogoutBtn_Click(object sender, RoutedEventArgs e)
        {
            var index = new Index();
            index.Show();

            Close();
        }
    }
}
