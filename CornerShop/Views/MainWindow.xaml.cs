using CornerShop.Core.Data;
using CornerShop.Core.Models;
using CornerShop.ViewModels;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace CornerShop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        private MainViewModel _viewModel;
        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainViewModel();
            _viewModel.OnLoginSuccess += (user) =>
            {
                OpenNextWindow(user);
            };
            this.DataContext = _viewModel;
        }
        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await _viewModel.LoginAsync(UserBox.Text, PasswordBox.Password);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("An error occurred during login: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        private async void Register_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               
                string username = UserBox.Text;
                string password = PasswordBox.Password;
                await _viewModel.RegisterAsync(username, password);

                MessageBox.Show("Registration successful! You can now log in.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void OpenNextWindow(User user)
        {
            var repo = new ShopRepository(DBConfig.ConnectionString);

            if (user.Role == UserRole.Admin)
            {
                OpenAdminApi();
                return;
            }
            else
            {
                var reopo = new ShopRepository(DBConfig.ConnectionString);
                var shopWindow = new ShopWindow(user, repo);
                shopWindow.Show();
            }
            this.Close();
        }
        private void OpenAdminApi()
        {
            try
            {
                MessageBox.Show("Logged in as Admin.\nOpening API Documentation...");
                string url = DBConfig.AdminApiUrl;

                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Failed to open browser: " + ex.Message);
            }
        }
    }
}