using System.Windows;

namespace DesktopApp
{
    public partial class Index : Window
    {
        public Index()
        {
            InitializeComponent();
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            var login = new Login();
            login.CodeReceived += Login_CodeReceived;

            login.ShowDialog();
        }

        private void Login_CodeReceived(object? sender, CodeReceivedEventArgs e)
        {
            var userProfile = new UserProfile(e.Code);
            userProfile.Show();

            Close();
        }
    }
}
