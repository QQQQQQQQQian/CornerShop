using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CornerShop.Core.Data;
using CornerShop.Core.Models;
using CornerShop.ViewModels;

namespace CornerShop.Views
{
    /// <summary>
    /// Interaction logic for CheckoutWindow.xaml
    /// </summary>
    public partial class CheckoutWindow : Window
    {
        private CheckoutViewModel viewModel;
        public CheckoutWindow(User user, ShopRepository repo)
        {
            InitializeComponent();
            viewModel=new CheckoutViewModel(user,repo);
            viewModel.OnPaymentSuccess += () =>
            {
                this.DialogResult = true;
                this.Close();
            };
            this.DataContext = viewModel;
        }
        private async void Pay_Click(object sender, RoutedEventArgs e)
        {
            await viewModel.PayAsync();
        }
    }
}
