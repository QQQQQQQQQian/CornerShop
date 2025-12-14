using CornerShop.Data;
using CornerShop.Models;
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

namespace CornerShop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ShopRepository _repo;
        public MainWindow()
        {
            InitializeComponent();
            _repo = new ShopRepository();
        }
        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameBox.Text;
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter username and password.");
                return;
            }

            var user = await _repo.LoginAsync(username, password);

            if (user == null)
            {
                MessageBox.Show("Invalid username or password.");
                return;
            }

            OpenNextWindow(user);
        }
        private async void Register_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var user = await _repo.RegisterAsync(
                    UsernameBox.Text,
                    PasswordBox.Password);

                MessageBox.Show("Registration successful!");
                OpenNextWindow(user);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OpenNextWindow(User user)
        {
            Window nextWindow;

            if (user.Role == UserRole.Admin)
            {
                nextWindow = new AdminWindow(user);
            }
            else
            {
                nextWindow = new ShopWindow(user);
            }

            nextWindow.Show();
            this.Close();
        }
    


    }
}